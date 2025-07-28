using HRSDataIntegration.DTOs;
using HRSDataIntegration.DTOs.Chart;
using HRSDataIntegration.DTOs.Personeli;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace HRSDataIntegration.EntityFrameworkCore.DataAnnotations
{
    
    public class OracleDbContext: AbpDbContext<OracleDbContext>
    {
        public DbSet<TBACTIVITY_LOG_CHARTDESIGN> TBACTIVITY_LOG_CHARTDESIGN { get; set; }

        public DbSet<TBJOB> TBJOB { get; set; }
        public DbSet<TBPOST> TBPOST { get; set; }
        public DbSet<TBPOST_JOB> TBPOST_JOB { get; set; }
        public DbSet<TBPROVINCE> TBPROVINCE { get; set; }
        public DbSet<TBPROVINCE_NAME_DETAIL> TBPROVINCE_NAME_DETAIL { get; set; }
        public DbSet<TBPROVINCE_POLI_PROVINCE_DTL> TBPROVINCE_POLI_PROVINCE_DTL { get; set; }
        public DbSet<TBUNIT> TBUNIT { get; set; }        
        public DbSet<TBCUNIT_TYPE> TBCUNIT_TYPE { get; set; }        
        public DbSet<TBUNIT_PARENT_DETAIL> TBUNIT_PARENT_DETAIL { get; set; }
        public DbSet<TBUNIT_PROVINCE_DETAIL> TBUNIT_PROVINCE_DETAIL { get; set; }
        public DbSet<TBUNIT_NAME> TBUNIT_NAME { get; set; }
        public DbSet<TBUNIT_TYPE_DETAIL> TBUNIT_TYPE_DETAIL { get; set; }
        public DbSet<TBCHART_TEMPLATE_NEW> TBCHART_TEMPLATE_NEW { get; set; }
        public DbSet<TBCHART_POST_TEMPLATE> TBCHART_POST_TEMPLATE { get; set; }
        public DbSet<TBCHART_LINK> TBCHART_LINK { get; set; }
        public DbSet<TBUNIT_DESTROY_DETAIL> TBUNIT_DESTROY_DETAIL { get; set; }
        public DbSet<TBNON_LIABILITY_REASON> TBNON_LIABILITY_REASON { get; set; }
        public DbSet<TBEDUCATION_STUDY> TBEDUCATION_STUDY { get; set; }
        public DbSet<TBEDUCATION_BRANCH> TBEDUCATION_BRANCH { get; set; }
        public DbSet<TBPERSONNEL_PENSION_FUND> TBPERSONNEL_PENSION_FUND { get; set; }
        public DbSet<TBCPENSION_FUND> TBCPENSION_FUND { get; set; }
        public DbSet<TBUNIVERSITY> TBUNIVERSITY { get; set; }
        public DbSet<TBCTAMIN_BRANCH> TBCTAMIN_BRANCH { get; set; }
        public DbSet<TBNON_DEPENDENT_REASON> TBNON_DEPENDENT_REASON { get; set; }
        public DbSet<TBFAMILY_GRADUATION> TBFAMILY_GRADUATION { get; set; }
        public DbSet<TBPERSONNEL> TBPERSONNEL { get; set; }
        public DbSet<TBFAMILY> TBFAMILY { get; set; }
        public DbSet<TBFAMILY_TOTAL> TBFAMILY_TOTAL { get; set; }
        public DbSet<TBFAMILY_MARRIAGE> TBFAMILY_MARRIAGE { get; set; }
        public DbSet<TBFAMILY_MOBILE> TBFAMILY_MOBILE { get; set; }
        public DbSet<TBPERSONNEL_TOTAL> TBPERSONNEL_TOTAL { get; set; }
        public DbSet<TBDEPENDENT> TBDEPENDENT { get; set; }
        public DbSet<TBFOREIGN_LANGUAGE> TBFOREIGN_LANGUAGE { get; set; }
        

        public OracleDbContext(DbContextOptions<OracleDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("HRS");
            base.OnModelCreating(modelBuilder);
        }
    }
}
