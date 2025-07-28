using HRSDataIntegration.DTOs;
using HRSDataIntegration.Interfaces;
using MassTransit.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRSDataIntegration.Services
{
    public class PostService : IPostService
    {
        private readonly IOracleCommon _oracleCommon;
        private readonly ISqlRepository<PostDetail> _sqlRepositoryPostDetail;
        private readonly ISqlRepository<PostJob> _sqlRepositoryPostJob;
        private readonly IOracleRepository<TBPOST> _postRepository;
        private readonly IOracleRepository<TBPOST_JOB> _postJobRepository;
        public PostService(IOracleCommon oracleCommon, ISqlRepository<PostDetail> sqlRepositoryPostDetail, IOracleRepository<TBPOST> postRepository,
            ISqlRepository<PostJob> sqlRepositoryPostJob,
            IOracleRepository<TBPOST_JOB> postJobRepository
            )
        {
            _oracleCommon = oracleCommon;
            _sqlRepositoryPostDetail = sqlRepositoryPostDetail;
            _postRepository = postRepository;
            _postJobRepository = postJobRepository;
            _sqlRepositoryPostJob = sqlRepositoryPostJob;
        }
        public void ConvertJobPostConvertToOracle(string Id)
        {
             var sqlPostDetail = _sqlRepositoryPostDetail.GetQueryable()
                 .Where(x=>x.PostId.ToString() == Id)
                 .Select(x=> new
                    {
                      ID = x.Post.Id,
                      CODE = x.Code,
                      NAME=x.Title,
                      POST_TYPE_CODE = x.PostLevelId ,
                      ACTIVE_TYPE_CODE= x.Post.IsActive,
                      STAR_POST_CODE = 2,
                      SPECIFIC_PARTS_CODE = 2, // قرار شد شنبه 27 اردیبهشت چک شود
                      CREATION_DATE = _oracleCommon.ToStringDateTime(x.EffectiveDateFrom),
                      POST_RASTEH_CODE = "",
                      RASTEHID = x.PostTypeId,
                      LEVELID = x.PostLevelId
                   }).FirstOrDefault();

            var OldColumnValueOfMappingId_RASTEH_Id = _oracleCommon.OldColumnValue("HRS.TBCPOST_RASTEH", "CODE", sqlPostDetail.RASTEHID.ToString());
            var OldColumnValueOfMappingId_LEVEL_Id = _oracleCommon.OldColumnValue("TBCPOST_TYPE", "CODE", sqlPostDetail.LEVELID.ToString());

            var TBPostData = new TBPOST()
            {
                ID = sqlPostDetail.ID.ToString(),
                CODE = sqlPostDetail.CODE,
                NAME = sqlPostDetail.NAME,
                POST_TYPE_CODE = int.Parse(OldColumnValueOfMappingId_LEVEL_Id),
                ACTIVE_TYPE_CODE = sqlPostDetail.ACTIVE_TYPE_CODE ? 1 : 0,
                STAR_POST_CODE = sqlPostDetail.STAR_POST_CODE,
                SPECIFIC_PARTS_CODE =sqlPostDetail.SPECIFIC_PARTS_CODE,
                CREATION_DATE = sqlPostDetail.CREATION_DATE,
                POST_RASTEH_CODE = int.Parse(OldColumnValueOfMappingId_RASTEH_Id),
            };

            _postRepository.Create(TBPostData);
            _postRepository.SaveChanges();

            _oracleCommon.TBActivity_Log("TBACTIVITY_LOG_CHARTDESIGN", Id, 1008, 8589934592);
            _oracleCommon.InsertInto_DataConverter_MappingId(sqlPostDetail.ID.ToString(), Id , "HRS.TBPOST","ID" , "OrganChart.Post","ID");
            var oldPostId = _oracleCommon.OldColumnValue("HRS.TBPOST", "ID", Id);
            var sqlPostJobQueryable = _sqlRepositoryPostJob.GetQueryable().
                Where(x => x.PostId.ToString() == Id)
                .Select(x => new TBPOST_JOB
                {
                    ID = x.Id.ToString(),
                    POST_ID = oldPostId,
                    JOB_ID = _oracleCommon.OldColumnValue("HRS.TBJOB", "ID", x.JobId.ToString())
                })
                .ToList();

            _postJobRepository.CreateList(sqlPostJobQueryable);
            _postJobRepository.SaveChanges();
          //  _oracleCommon.InsertInto_DataConverter_MappingId(sqlPostDetail.ID.ToString(), Id, "HRS.TBPOST_JOB", "ID", "OrganChart.Post", "ID");            
        }

        public void UpdatePostJob(string Id) //PostId
        {
            var postJobs = _sqlRepositoryPostJob.GetQueryable()
                .Where(x => x.PostId.ToString() == Id)
                .Select(x => new TBPOST_JOB
                {
                    ID = x.Id.ToString(),
                    POST_ID = _oracleCommon.OldColumnValue("HRS.TBPOST" , "ID" , x.PostId.ToString()),
                    JOB_ID = _oracleCommon.OldColumnValue("HRS.TBJOB" , "ID" , x.JobId.ToString())
                }).ToList();

            var TBPOST_JOB  = _postJobRepository.GetQueryable()
                .Where(x => x.POST_ID.ToString() == _oracleCommon.OldColumnValue("HRS.TBPOST", "ID", Id.ToString())).
                Select(x=>new TBPOST_JOB
                {
                    ID =x.ID,
                    POST_ID=x.POST_ID,
                    JOB_ID = x.JOB_ID
                }).ToList();

            var existInSql = postJobs.Except(TBPOST_JOB, new PostJobComparer()).ToList(); 
            if (existInSql.Count > 0)
            {
                _postJobRepository.CreateList(existInSql);
                _postJobRepository.SaveChanges();
            }

            var existInOracle = TBPOST_JOB.Except(postJobs, new PostJobComparer()).ToList();
            if (existInOracle.Count > 0)
            {
                _postJobRepository.DeleteList(existInOracle);
                _postJobRepository.SaveChanges();
            }
        }
    }

    class PostJobComparer : IEqualityComparer<TBPOST_JOB>
    {
        public bool Equals(TBPOST_JOB x, TBPOST_JOB y)
        {
            return x.ID == y.ID; // مقایسه بر اساس `Id`
        }

        public int GetHashCode([DisallowNull] TBPOST_JOB obj)
        {
            return obj.ID.GetHashCode();
        }
    }
}
