using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Chart;
using HRSDataIntegration.EntityFrameworkCore;
using HRSDataIntegration.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.Util.ChartTable;

namespace HRSDataIntegration.Services
{
    public class ChartService : IChartService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly IOracleRepository<TBCHART_TEMPLATE_NEW> _TBCHART_TEMPLATE_NEW;
        private readonly IOracleRepository<TBCHART_POST_TEMPLATE> _TBCHART_POST_TEMPLATE;
        private readonly IOracleRepository<TBCHART_LINK> _TBCHART_LINK;
        private readonly IOracleRepository<TBUNIT> _TBUnit;
        private readonly IOracleRepository<TBPROVINCE> _TBProvince;
        private readonly ISqlRepository<OrganizationChartNodeDetail> _sqlRepositoryOrganizationChartNodeDetail;
        private readonly ISqlRepository<OrganizationChart> _sqlRepositoryOrganizationChart;
        private readonly ISqlRepository<OrganizationChartLimitation> _sqlRepositoryOrganizationChartLimitation;

        public ChartService(IOracleCommon oracleCommon, 
            IOracleRepository<TBCHART_TEMPLATE_NEW> TBCHART_TEMPLATE_NEW,
            IOracleRepository<TBCHART_POST_TEMPLATE> TBCHART_POST_TEMPLATE,
            IOracleRepository<TBCHART_LINK> TBCHART_LINK,
            ISqlRepository<OrganizationChartNodeDetail> sqlRepositoryOrganizationChartNodeDetail,
            ISqlRepository<OrganizationChart> sqlRepositoryOrganizationChart,
            ISqlRepository<OrganizationChartLimitation> sqlRepositoryOrganizationChartLimitation, 
            IOracleRepository<TBUNIT> tBUnit, 
            IOracleRepository<TBPROVINCE> tBProvince)
        {
            _oracleCommon = oracleCommon;
            _TBCHART_TEMPLATE_NEW = TBCHART_TEMPLATE_NEW;
            _TBCHART_POST_TEMPLATE = TBCHART_POST_TEMPLATE;
            _TBCHART_LINK = TBCHART_LINK;
            _sqlRepositoryOrganizationChartNodeDetail = sqlRepositoryOrganizationChartNodeDetail;
            _sqlRepositoryOrganizationChart = sqlRepositoryOrganizationChart;
            _sqlRepositoryOrganizationChartLimitation = sqlRepositoryOrganizationChartLimitation;
            _TBUnit = tBUnit;
            _TBProvince = tBProvince;
        }

        public List<OrganizationChartNodeDetail> GetChildrenRecursive(List<OrganizationChartNodeDetail> allRecords, Guid parentId)
        {
            
            var children = allRecords.Where(x => x.ParentId == parentId && x.PostId != null).ToList();
            var result = new List<OrganizationChartNodeDetail>(children);
            
            if (children.Any())
            {
                foreach (var child in children)
                {
                    result.AddRange(GetChildrenRecursive(allRecords, child.Id));
                }
            }
            return result;
        }


        public void ConvertSqlChartTable_Insert_ToOracletable(string Id) //Id is UnitId
        {
            try
            {
                #region get queryable of OrganizationChartNodeDetail
                var OrganizationChartNodeDetailQueryable = _sqlRepositoryOrganizationChartNodeDetail.GetQueryable();
                var chartTemplate = OrganizationChartNodeDetailQueryable.Select(x => new
                {
                    Id = x.Id,
                    ApproveDate = _oracleCommon.ToStringDateTime(x.OrganizationChart.EffectiveDate),
                    Domain_Code = 8589934592,
                    UnitId = x.UnitId,
                    ApproveNumber = x.OrganizationChart.ApproveNumber == "" ? "1" : "0",
                    StateCode = x.OrganizationChart.StateCode,
                    Description = x.OrganizationChart.Description,
                    OrganizationChartId = x.OrganizationChartId
                }).Where(x => x.UnitId.ToString() == Id).FirstOrDefault();
                #endregion get queryable of OrganizationChartNodeDetail

                var oldUnit_ID = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", chartTemplate.UnitId.ToString());

                #region insert into TBCHART_TEMPLATE_NEW
                var TBCHART_TEMPLATE_NEW = new TBCHART_TEMPLATE_NEW()
                {
                    ID = chartTemplate.Id.ToString(),
                    APPROVED_DATE = chartTemplate.ApproveDate,
                    ORIGIN_NO = chartTemplate.ApproveNumber,
                    UNIT_ID = oldUnit_ID,
                    DOMAIN_CODE = 8589934592,
                    ORIGIN_CODE = 1,
                    STATE_CODE = 4,//chartTemplate.StateCode,
                    LEFT_MARGIN = 20,
                    TOP_MARGIN = 20,
                    BACK_COLOR = -1,
                    CHART_DESCRIPTION = chartTemplate.Description,
                    AUTO_DRAW_CODE = 1,
                    CHART_TYPE_CODE = 1,
                    THEME_ID = "f542b146-1a21-4ed4-b2a6-726e7e98fee5",
                    FONT_SIZE = 8,
                    CURRENT_PAPER_CODE = 6,
                    FONT_NAME = "Tahoma",
                    FONT_STYLE = 0,
                    FOR_COLOR = "-16777216",
                };

                _TBCHART_TEMPLATE_NEW.Create(TBCHART_TEMPLATE_NEW);
                _TBCHART_TEMPLATE_NEW.SaveChanges();
                #endregion insert into TBCHART_TEMPLATE_NEW

                #region Fetch OrganizationChartNodeDetail by unitid
                var hierarchyRecordsWithUnit = OrganizationChartNodeDetailQueryable
                    .Include(x => x.Parent)
                    .Where(x => x.OrganizationChartId == chartTemplate.OrganizationChartId
                             && x.UnitId != null
                    ).FirstOrDefault();
                var hierarchyRecordsWithPost = OrganizationChartNodeDetailQueryable
                    .Include(x => x.Parent)
                    .Where(x => x.OrganizationChartId == chartTemplate.OrganizationChartId
                     && x.PostId != null
                    )
                    //.OrderByDescending(x => x.ParentId == hierarchyRecordsWithUnit.Id);
                    .ToList();
                var ParentNode = hierarchyRecordsWithPost.FirstOrDefault(x => x.ParentId == hierarchyRecordsWithUnit.Id).Id;
                #endregion Fetch OrganizationChartNodeDetail by unitid
                var allChildren = GetChildrenRecursive(hierarchyRecordsWithPost, ParentNode);
                #region Sort By parentChild Relation 
                var parent = hierarchyRecordsWithPost.FirstOrDefault(x => x.Id == ParentNode);
                allChildren.Insert(0, parent);
                #endregion Sort By parentChild Relation 

                foreach (var postRecord in allChildren)
                {
                    #region insert parent into TBCHART_POST_TEMPLATE
                    var tbCharPostTemplate = new TBCHART_POST_TEMPLATE
                    {
                        ID = postRecord.Id.ToString(),
                        APPROVED_COUNT = 1, //x.OrganizationChart.OrganizationChartLimitations.Select(x => x.Quantity).FirstOrDefault(),
                        CHART_TEMPLATE_ID = TBCHART_TEMPLATE_NEW.ID,
                        PARENT_POST_ID = postRecord.ParentId.ToString() == hierarchyRecordsWithUnit.Id.ToString() ? null : postRecord.Parent.Id.ToString(),
                        POST_ID = _oracleCommon.OldColumnValue("HRS.TBPOST", "ID", postRecord.PostId.ToString()),
                        DESCRIPTION = postRecord.Parent.Description,
                        WIDTH = postRecord.OrganizationChartNodeDiagrams.Select(x => (int?)Convert.ToInt32(x.Width)).FirstOrDefault(),
                        HEIGHT = postRecord.OrganizationChartNodeDiagrams.Select(x => (int?)Convert.ToInt32(x.Height)).FirstOrDefault(),
                        CHILD_INDEX = postRecord.Order,
                        X_CORDINATE = postRecord.OrganizationChartNodeDiagrams.Select(x => Convert.ToInt32(x.xPos)).FirstOrDefault(),
                        Y_CORDINATE = postRecord.OrganizationChartNodeDiagrams.Select(x => Convert.ToInt32(x.yPos)).FirstOrDefault(),
                        RADIF = postRecord.Radif,
                        FONT_NAME = "Tahoma",
                        FONT_SIZE = 8,
                        FONT_STYLE = 1,
                        FONT_COLOR = "-16777216",
                        DOMAIN_CODE = 8589934592,
                        F_P_CONNECTOR_WIDTH = 0,
                        T_C_CONNECTOR_WIDTH = 0,
                        T_C_CONNECTOR_DASH_STYLE_CODE = 0,
                        T_C_CONNECTOR_COLOR = 0,
                        DRAW_GRADIANT = 0,
                        BORDER_COLOR = -16777216,
                        BORDER_DASH_STYLE_CODE = 0,
                        BORDER_WIDTH = 2,
                        SHAPE_TYPE_CODE = 1,
                        SHOW_CHILDS_CODE = 1,
                        POST_POSITION_TYPE_CODE = 1,
                        CHART_BOX_TYPE_CODE = 1,
                        BACK_COLOR2 = "0",
                        OPACITY = 100,
                        CONNECTOR_TYPE_CODE = 1,
                        F_P_CONNECTOR_DASH_STYLE_CODE = 0,
                        TEXT_ALIGNMENT_TYPE_CODE = 2,
                        COUNT_DISPLAY_CODE = 2,
                        BACK_COLOR = "-657931",
                        F_P_CONNECTOR_COLOR = "0",
                        CHILD_LAYOUT_TYPE_CODE = 1
                    };
                    _TBCHART_POST_TEMPLATE.Create(tbCharPostTemplate);
                    _TBCHART_POST_TEMPLATE.SaveChanges();
                    #endregion insert parent into TBCHART_POST_TEMPLATE

                    #region insert  into TBCHART_LINK

                    if (tbCharPostTemplate.PARENT_POST_ID != null)
                    {
                        var link = new TBCHART_LINK()
                        {
                            ID = Guid.NewGuid().ToString(),
                            CHART_ID = TBCHART_TEMPLATE_NEW.ID,
                            START_CON_CODE = parent.OrganizationChartNodeDiagrams.Select(x => int.Parse(x.FromPointIndex)).FirstOrDefault(),//Convert.ToInt32(postRecord.OrganizationChartNodeDiagrams.FirstOrDefault().FromPointIndex),
                            END_CON_CODE = parent.OrganizationChartNodeDiagrams.Select(x => int.Parse(x.ToPointIndex)).FirstOrDefault(),//Convert.ToInt32(postRecord.OrganizationChartNodeDiagrams.FirstOrDefault().ToPointIndex),
                            PARENT_NODE_ID = postRecord.Parent.Id.ToString(),
                            CHILD_NODE_ID = postRecord.Id.ToString(),
                            COLOR = "-16777216",
                            WIDTH = 2,
                            DASH_STYLE_CODE = 0,
                            DRAW_GRADIANT = 0,
                            CONNECTOR_TYPE_CODE = 2,
                            OPACITY = 100,
                            FONT_COLOR = "-16777216",
                            FONT_NAME = "Microsoft Sans Serif",
                            FONT_SIZE = 10,
                            FONT_STYLE = 0,
                            AUTODRAW = 1,
                            DOMAIN_CODE = 8589934592
                        };
                        _TBCHART_LINK.Create(link);
                        _TBCHART_LINK.SaveChanges();
                    }
                    #endregion insert into TBCHART_LINK
                }
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id) , true);
            }
            catch (Exception ex)
            {
                string message = ex.Message?.Length > 500
                                ? ex.Message.Substring(0, 500)
                                : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id), false, message);
                throw ex;
            }
        }

        public void ConvertSqlChartTable_Update_ToOracletable(string Id) //Id is OrganizationChartId
        {
            try
            {
                var organChart = _sqlRepositoryOrganizationChart.GetQueryable()
                                     .Select(x => new { EffecitveDate = x.EffectiveDate, Id = x.Id })
                                     .Where(x => x.Id.ToString() == Id).FirstOrDefault();

                var organChartlimitationQueryable = _sqlRepositoryOrganizationChartLimitation.GetQueryable();

                var limitationUnit = organChartlimitationQueryable.Where(x => x.OrganizationChartId.ToString() == Id)
                                    .Select(x => x.UnitId)
                                    .Distinct()
                                    .ToList();

                foreach (var unitId in limitationUnit)
                {
                    var organChartNodeDetailQueryable = _sqlRepositoryOrganizationChartNodeDetail.GetQueryable()
                                                                                .Include(x=> x.OrganizationChart);
                    var organizationChartNodeDetailUnit = organChartNodeDetailQueryable
                        .Where(x =>
                                  x.UnitId == unitId && x.EffectiveDateFrom <= organChart.EffecitveDate
                                  && (x.EffectiveDateTo > organChart.EffecitveDate || x.EffectiveDateTo == 0)
                              )
                        .FirstOrDefault();

                    //var organizationChartNodeDetailParentPostPath = organChartNodeDetailQueryable
                    //    .Where(x => x.PostId != null && x.ParentId == organizationChartNodeDetailUnit.Id
                    //                        && x.EffectiveDateFrom <= organChart.EffecitveDate
                    //                                  && (x.EffectiveDateTo > organChart.EffecitveDate || x.EffectiveDateTo == 0)
                    //    )
                    //    .FirstOrDefault()
                    //    .ParentPath
                    //    .ToString();

                    //var activeOrganizationChartDetail = organChartNodeDetailQueryable.
                    //    Where(ocnd =>
                    //            EF.Functions.Like(ocnd.ParentPath.ToString(), organizationChartNodeDetailParentPostPath)

                    //            && ocnd.PostId != null
                    //            && ocnd.EffectiveDateFrom <= organChart.EffecitveDate
                    //            && (ocnd.EffectiveDateTo > organChart.EffecitveDate || ocnd.EffectiveDateTo == 0));

                    var oldUnit_ID = _oracleCommon.OldColumnValue("HRS.TBUNIT", "ID", organizationChartNodeDetailUnit.UnitId.ToString());

                    #region insert into TBCHART_TEMPLATE_NEW

                    var oldUnitId = _oracleCommon.OldColumnValue("HRS.TBUnit", "ID", organizationChartNodeDetailUnit.UnitId.ToString());

                    var oldUnit = _TBUnit.GetQueryable().Where(x=> x.ID == oldUnitId).ToList().FirstOrDefault();
                    var oldProvicne = _TBProvince.GetQueryable().Where(x => x.ID == oldUnit.PROVINCE_ID).ToList().FirstOrDefault();

                    var TBCHART_TEMPLATE_NEW = new TBCHART_TEMPLATE_NEW()
                    {
                        ID = Guid.NewGuid().ToString(),
                        APPROVED_DATE = _oracleCommon.ToStringDateTime(organChart.EffecitveDate),
                        ORIGIN_NO = organizationChartNodeDetailUnit.OrganizationChart.ApproveNumber,
                        DOMAIN_CODE = oldProvicne.DOMAIN_CODE,
                        UNIT_ID = oldUnit_ID,
                        ORIGIN_CODE = 1,
                        STATE_CODE = 4,
                        LEFT_MARGIN = 20,
                        TOP_MARGIN = 20,
                        BACK_COLOR = -1,
                        CHART_DESCRIPTION = organizationChartNodeDetailUnit.Description,
                        AUTO_DRAW_CODE = 0,
                        CHART_TYPE_CODE = 1,
                        THEME_ID = "19d88eab-6143-49f7-8b1d-bc0959fc8c90",
                        CURRENT_PAPER_CODE = 6,
                        FONT_SIZE = 8,
                        FONT_NAME = "Tahoma",
                        FONT_STYLE = 0,
                        FOR_COLOR = "-16777216",
                    };



                    #endregion insert into TBCHART_TEMPLATE_NEW

                    //var hierarchyRecordsWithUnit = organChartNodeDetailQueryable
                    //.Include(x => x.Parent)
                    //.Where(x => x.OrganizationChartId == organizationChartNodeDetailUnit.OrganizationChartId
                    //         && x.UnitId != null)
                    //.FirstOrDefault();

                    var hierarchyRecordsWithPost = 
                            organChartNodeDetailQueryable
                        .Include(x=> x.OrganizationChartNodeDiagrams)
                        .Include(x => x.Parent)
                            .ThenInclude(x => x.OrganizationChartNodeDiagrams)
                        .Where(x =>
                                    x.ParentPath.IsDescendantOf(organizationChartNodeDetailUnit.ParentPath)
                                    //HRSDataIntegrationDbContext.IsDescendantOf(x.ParentPath, organizationChartNodeDetailUnit.ParentPath)
                                    && x.EffectiveDateFrom <= organChart.EffecitveDate
                                    && (x.EffectiveDateTo > organChart.EffecitveDate || x.EffectiveDateTo == 0)
                                  )
                        .ToList();

                    var ParentNode = hierarchyRecordsWithPost.FirstOrDefault(x => x.ParentId == organizationChartNodeDetailUnit.Id && x.PostId != null).Id;

                    var allChildren = GetChildrenRecursive(hierarchyRecordsWithPost, ParentNode);

                    #region Sort By parentChild Relation 

                    var parent = hierarchyRecordsWithPost.FirstOrDefault(x => x.Id == ParentNode);

                    allChildren.Insert(0, parent);

                    #endregion Sort By parentChild Relation 

                    var chartPostTemplateList = new List<TBCHART_POST_TEMPLATE>();
                    var chartLinkList = new List<TBCHART_LINK>();
                    var chartPostTemplateMapList = new List<ChartPostTemplateMap>();

                    foreach (var postRecord in allChildren)
                    {
                        #region insert parent into TBCHART_POST_TEMPLATE

                        var newId = Guid.NewGuid().ToString();

                        chartPostTemplateMapList.Add(new ChartPostTemplateMap() 
                        { 
                            Id = newId,
                            PostId = postRecord.Id.ToString(),
                            ParentPostId = postRecord.Parent.Id.ToString(),
                        });

                        var tbCharPostTemplate = new TBCHART_POST_TEMPLATE
                        {
                            ID = newId,
                            APPROVED_COUNT = 1, //x.OrganizationChart.OrganizationChartLimitations.Select(x => x.Quantity).FirstOrDefault(),
                            CHART_TEMPLATE_ID = TBCHART_TEMPLATE_NEW.ID,
                            PARENT_POST_ID = postRecord.ParentId.ToString() == organizationChartNodeDetailUnit.Id.ToString()
                                                                ? null
                                                                : postRecord.Parent.Id.ToString(),
                            POST_ID = _oracleCommon.OldColumnValue("HRS.TBPOST", "ID", postRecord.PostId.ToString()),
                            DESCRIPTION = postRecord.Parent.Description,
                            WIDTH = postRecord.OrganizationChartNodeDiagrams.Select(x => (int?)Convert.ToInt32(x.Width)).FirstOrDefault(),
                            HEIGHT = postRecord.OrganizationChartNodeDiagrams.Select(x => (int?)Convert.ToInt32(x.Height)).FirstOrDefault(),
                            CHILD_INDEX = postRecord.Order,
                            X_CORDINATE = postRecord.OrganizationChartNodeDiagrams.Select(x => Convert.ToInt32(x.xPos)).FirstOrDefault(),
                            Y_CORDINATE = postRecord.OrganizationChartNodeDiagrams.Select(x => Convert.ToInt32(x.yPos)).FirstOrDefault(),
                            RADIF = postRecord.Radif,
                            FONT_NAME = "Tahoma",
                            FONT_SIZE = 8,
                            FONT_STYLE = 1,
                            FONT_COLOR = "-16777216",
                            DOMAIN_CODE = 8589934592,
                            F_P_CONNECTOR_WIDTH = 0,
                            T_C_CONNECTOR_WIDTH = 0,
                            T_C_CONNECTOR_DASH_STYLE_CODE = 0,
                            T_C_CONNECTOR_COLOR = 0,
                            DRAW_GRADIANT = 0,
                            BORDER_COLOR = -16777216,
                            BORDER_DASH_STYLE_CODE = 0,
                            BORDER_WIDTH = 2,
                            SHAPE_TYPE_CODE = 1,
                            SHOW_CHILDS_CODE = 1,
                            POST_POSITION_TYPE_CODE = 1,
                            CHART_BOX_TYPE_CODE = 1,
                            BACK_COLOR2 = "0",
                            BACK_COLOR = "-657931",
                            OPACITY = 100,
                            CHILD_LAYOUT_TYPE_CODE = 1,
                            CONNECTOR_TYPE_CODE = 1,
                            F_P_CONNECTOR_DASH_STYLE_CODE = 0,
                            F_P_CONNECTOR_COLOR = "0",
                            TEXT_ALIGNMENT_TYPE_CODE = 2,
                            COUNT_DISPLAY_CODE = 2
                        };
                        chartPostTemplateList.Add(tbCharPostTemplate);

                        #endregion insert parent into TBCHART_POST_TEMPLATE

                        #region insert  into TBCHART_LINK

                        if (tbCharPostTemplate.PARENT_POST_ID != null)
                        {
                            var link = new TBCHART_LINK()
                            {
                                ID = Guid.NewGuid().ToString(),
                                CHART_ID = TBCHART_TEMPLATE_NEW.ID,
                                START_CON_CODE = ConvertNodePointIndexToOldSystem(postRecord.OrganizationChartNodeDiagrams.Select(x => int.Parse(x.FromPointIndex)).FirstOrDefault()),
                                END_CON_CODE = ConvertNodePointIndexToOldSystem(postRecord.OrganizationChartNodeDiagrams.Select(x => int.Parse(x.ToPointIndex)).FirstOrDefault()),
                                PARENT_NODE_ID = postRecord.Parent.Id.ToString(),
                                CHILD_NODE_ID = postRecord.Id.ToString(),
                                COLOR = "-16777216",
                                WIDTH = 2,
                                DASH_STYLE_CODE = 0,
                                DRAW_GRADIANT = 0,
                                CONNECTOR_TYPE_CODE = 2,
                                OPACITY = 100,
                                FONT_COLOR = "-16777216",
                                FONT_NAME = "Microsoft Sans Serif",
                                FONT_SIZE = 10,
                                FONT_STYLE = 0,
                                //POINTS_DATA = "",
                                AUTODRAW = 1,
                                DOMAIN_CODE = 8589934592
                            };
                            chartLinkList.Add(link);
                        }
                        #endregion insert into TBCHART_LINK
                    }

                    foreach (var chartLink in chartLinkList)
                    {
                        var currentChartPost = chartPostTemplateMapList.FirstOrDefault(x => x.PostId == chartLink.CHILD_NODE_ID);
                        var parentChartPost = chartPostTemplateMapList.FirstOrDefault(x => x.PostId == chartLink.PARENT_NODE_ID);

                        chartLink.CHILD_NODE_ID = currentChartPost.Id.ToString();
                        chartLink.PARENT_NODE_ID = parentChartPost.Id.ToString();
                    }

                    foreach (var chartPost in chartPostTemplateList)
                    {
                        if (chartPost.PARENT_POST_ID is not null)
                        {
                            var parentChartPost = chartPostTemplateMapList.FirstOrDefault(x => x.PostId == chartPost.PARENT_POST_ID);
                            chartPost.PARENT_POST_ID = parentChartPost.Id;
                        }
                    }

                    _TBCHART_TEMPLATE_NEW.Create(TBCHART_TEMPLATE_NEW);

                    if (chartPostTemplateList.Count > 0)
                    {
                        _TBCHART_POST_TEMPLATE.CreateList(chartPostTemplateList);
                    }

                    if (chartLinkList.Count > 0)
                    {
                        _TBCHART_LINK.CreateList(chartLinkList);
                    }
                    

                    _TBCHART_TEMPLATE_NEW.SaveChanges();
                    _TBCHART_POST_TEMPLATE.SaveChanges();
                    _TBCHART_LINK.SaveChanges();
                }
                
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id), true);
            }
            catch (Exception ex)
            {
                string message = ex.Message?.Length > 500
                                ? ex.Message.Substring(0, 500)
                                : ex.Message;
                _oracleCommon.UpdateDataSyncLog(Guid.Parse(Id), false, message);
                throw ex;
            }
        }

        public int ConvertNodePointIndexToOldSystem(int NewPointIndex)
        {
            int oldPointIndex;

            switch (NewPointIndex)
            {
                case 0: 
                    {
                        oldPointIndex = 0;
                        break;
                    }
                case 1:
                    {
                        oldPointIndex = 3;
                        break;
                    }
                case 2:
                    {
                        oldPointIndex = 1;
                        break;
                    }
                case 3:
                    {
                        oldPointIndex = 2;
                        break;
                    }
                default: 
                    {
                        oldPointIndex = 0;
                            break;
                    }
            }

            return oldPointIndex;
        }
    }

    public class ChartPostTemplateMap 
    {
        public string Id { get; set; }
        public string PostId { get; set; }
        public string ParentPostId { get; set; }
    }
}




