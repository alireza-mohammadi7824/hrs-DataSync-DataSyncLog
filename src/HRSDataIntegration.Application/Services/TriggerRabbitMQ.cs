using HRSDataIntegration.DTOs;
using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.BackgroundJobs;
using MassTransit;
namespace HRSDataIntegration.Services
{
    public class TriggerRabbitMQ : ApplicationService
    {

        private readonly IBus _busService;
        public TriggerRabbitMQ(IBus busService)
        {
            _busService = busService;
        }

        public async Task SendJobRabbitMQMessage()
        {
            await _busService.Publish<MessageDTO>(new MessageDTO { Id = "7A429CEE-F46E-EFD8-7508-3A16C10AE95C", Type = "Insert_Job_Queue" });
        }

        public async Task SendPostRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "AFDFD001-4F81-EC30-9E5A-3A16EABE548A", Type = "Insert_POST_Queue" });
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "91C2267E-614B-4255-B175-A1CCCBDE95A5", Type = "Insert_POST_Queue" });
        }
        public async Task SendUpdatePostJobRabbitMQMessage()
        {
            // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "AFDFD001-4F81-EC30-9E5A-3A16EABE548A", Type = "Insert_POST_Queue" });
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "91C2267E-614B-4255-B175-A1CCCBDE95A5", Type = "Update_POST_JOB_Queue" });
        }

        public async Task SendUnitRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Insert_UNIT_Queue" });
        }

        public async Task SendProvinceInsertRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "84DC5C22-16E8-E3D9-8544-3A19F9EF3CEA", Type = "Insert_PROVINCE_Queue" });
        }
        public async Task SendProvinceUpdateRabbitMQMessage()
        {
          // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "84DC5C22-16E8-E3D9-8544-3A19F9EF3CEA", Type = "Update_PROVINCE_Queue" });
        }
        public async Task SendProvinceDeleteRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "84DC5C22-16E8-E3D9-8544-3A19F9EF3CEA", Type = "Delete_PROVINCE_Queue" });
        }
        public async Task SendChartRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "78CF45B7-5336-4588-DC13-3A19B33F499E", Type = "Insert_CHART_Queue" });
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "4870C49D-C372-E7AE-321F-3A19B33F49CD", Type = "Update_CHART_Queue" });
        }
        public async Task SendUnitParentUpdateRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Update_UnitParentId_Queue" });
        }

        public async Task SendUnitNAMEUpdateRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Update_UnitName_Queue" });
        }
        public async Task SendUnitAddressUpdateRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Update_UnitAddress_Queue" });
        }
        public async Task SendUnitTelsUpdateRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Update_UnitTels_Queue" });
        }
        public async Task SendUnitDestroy_Edgham_RabbitMQMessage()
        {
            //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Insert_UnitDestroyEdgham_Queue" });
        }
        public async Task SendUnitDestroy_Enhelal_RabbitMQMessage()
        {
           //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8AF1E1F0-2B9C-36E8-3247-3A19F9F245A5", Type = "Insert_UnitDestroyEnhelal_Queue" });
        }
        public async Task SendInsertUnitLevelRabbitMQMessage()
        {
           //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Insert_UnitLevel_Queue" });
        }
        public async Task SendUpdateUnitLevelRabbitMQMessage()
        {
            
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Update_UnitLevel_Queue" });
        }
        public async Task SendDeleteUnitLevelRabbitMQMessage()
        {
           
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Delete_UnitLevel_Queue" });
        }
        public async Task SendInsertStudyFieldRabbitMQMessage()
        {
           
         //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "D5384F3A-7E05-4662-9BB8-69F1433895D5", Type = "Insert_StudyField_Queue" });
        }
        public async Task SendUpdateStudyFieldRabbitMQMessage()
        {
           
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "D5384F3A-7E05-4662-9BB8-69F1433895D5", Type = "Update_StudyField_Queue" });
        }
        public async Task SendDeleteStudyFieldRabbitMQMessage()
        {
           
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "D5384F3A-7E05-4662-9BB8-69F1433895D5", Type = "Delete_StudyField_Queue" });
        }
        public async Task SendInsertLanguageRabbitMQMessage()
        {
           
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "A4BF735D-86B8-2CB9-6F92-3A16C0D65773", Type = "Insert_Language_Queue" });
        }
        public async Task SendUpdateLanguageRabbitMQMessage()
        {
            
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Update_Language_Queue" });
        }
        public async Task SendDeleteLanguageRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Delete_Language_Queue" });
        }
        public async Task SendInsertPensionFundRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8B55FAF0-230A-472D-87F3-0F257B747CDE", Type = "Insert_PensionFund_Queue" });
        }
        public async Task SendUpdatePensionFundRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8B55FAF0-230A-472D-87F3-0F257B747CDE", Type = "Update_PensionFund_Queue" });
        }
        public async Task SendDeletePensionFundRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8B55FAF0-230A-472D-87F3-0F257B747CDE", Type = "Delete_PensionFund_Queue" });
        }
        public async Task SendInsertPensionFundBranchRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "3D59A662-28BE-3FDD-88B6-3A1ACC153158", Type = "Insert_PensionFundBranch_Queue" });
        }
        public async Task SendUpdatePensionFundBranchRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "3D59A662-28BE-3FDD-88B6-3A1ACC153158", Type = "Update_PensionFundBranch_Queue" });
        }
        public async Task SendDeletePensionFundBranchRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "3D59A662-28BE-3FDD-88B6-3A1ACC153158", Type = "Delete_PensionFundBranch_Queue" });
        }
        public async Task SendInsertPersonDetailRabbitMQMessage()
        {
            //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "6C11A13E-917B-92BA-611E-3A1A145B5DC2", Type = "Insert_PersonDetail_Queue" });
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "2B5C751D-366C-8D7D-1596-3A1A145B798B", Type = "Insert_PersonDetail_Queue" });
        }
        public async Task SendDivorceRegistrationProcessForPersonelRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "B12738EF-03BF-D48D-45DF-3A195F1E9469", Type = "DivorceRegistrationProcessForPersonel_Queue" });
        }
        public async Task SendPersonelMarriageProcessRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "PersonelMarriageProcess_Queue" });
        }
        public async Task SendProcessOfRegisteringDeathOfPersonelRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "ProcessOfRegisteringDeathOfPersonel_Queue" });
        }
        public async Task SendProcessOfRegistrationBirthOfEmployeesChildreanRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "ProcessOfRegistrationBirthOfEmployeesChildrean_Queue" });
        }
        public async Task SendProcessOfRegistrationMarriageOfEmployeesChildreanRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "ProcessOfRegistrationMarriageOfEmployeesChildrean_Queue" });
        }
        public async Task SendInsertUniversityRabbitMQMessage()
        {
             //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "Insert_University_Queue" });
        }
        public async Task SendUpdateUniversityRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "Update_University_Queue" });
        }
        public async Task SendDeleteUniversityRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C0DAF8D7-B46E-46E2-88CB-0F0369648AC0", Type = "Delete_University_Queue" });
        }
        public async Task SendInsertUniversityTypeRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Insert_UniversityType_Queue" });
        }
        public async Task SendUpdateUniversityTypeRabbitMQMessage()
        {
           //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Update_UniversityType_Queue" });
        }
        public async Task SendDeleteUniversityTypeRabbitMQMessage()
        { 
            //await _busService.Publish<MessageDTO>(new MessageDTO { Id = "FAD9104D-ECEF-48EA-9592-76F65CABB32D", Type = "Delete_UniversityType_Queue" });
        }
        public async Task SendInsertNotDependentReasonRabbitMQMessage()
        {
         //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8E3B3C68-B75F-3B17-ED73-3A1AD27746F0", Type = "Insert_NotDependentReason_Queue" });
        }
        public async Task SendUpdateNotDependentReasonRabbitMQMessage()
        {
         //    await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8E3B3C68-B75F-3B17-ED73-3A1AD27746F0", Type = "Update_NotDependentReason_Queue" });
        }
        public async Task SendDeleteNotDependentReasonRabbitMQMessage()
        {
            // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "8E3B3C68-B75F-3B17-ED73-3A1AD27746F0", Type = "Delete_NotDependentReason_Queue" });
        }
        public async Task SendPersonContactRabbitMQMessage()
        {
         //    await _busService.Publish<MessageDTO>(new MessageDTO { Id = "4AA29E0A-9404-B4F4-7745-3A1A145B6B0D", Type = "PersonContact_Queue" });
        }
        public async Task SendInsertNotFamilyRightReasonRabbitMQMessage()
        {
         //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "649BD1F0-F252-4337-8562-FB84C3F4E271", Type = "Insert_NotFamilyRightReason_Queue" });
        }
        public async Task SendUpdateNotFamilyRightReasonRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "649BD1F0-F252-4337-8562-FB84C3F4E271", Type = "Update_NotFamilyRightReason_Queue" });
        }
        public async Task SendDeleteNotFamilyRightReasonRabbitMQMessage()
        {
           //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "649BD1F0-F252-4337-8562-FB84C3F4E271", Type = "Delete_NotFamilyRightReason_Queue" });
        }
        public async Task SendInsertEmployeePensionFundRabbitMQMessage()
        {
          //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "25987d68-899e-c6ba-40bd-3a1a147f86e8", Type = "Insert_EmployeePensionFund_Queue" });
        }
        public async Task SendUpdateEmployeePensionFundRabbitMQMessage()
        {
           // await _busService.Publish<MessageDTO>(new MessageDTO { Id = "25987d68-899e-c6ba-40bd-3a1a147f86e8", Type = "Update_EmployeePensionFund_Queue" });
        }
        public async Task SendDeleteEmployeePensionFundRabbitMQMessage()
        {
          //  await _busService.Publish<MessageDTO>(new MessageDTO { Id = "25987d68-899e-c6ba-40bd-3a1a147f86e8", Type = "Delete_EmployeePensionFund_Queue" });
        }
        public async Task SendInsertPersonEducationRabbitMQMessage()
        {
           //   await _busService.Publish<MessageDTO>(new MessageDTO { Id = "C6C1B70D-38D6-E052-405C-3A1A145E86D8", Type = "Insert_PersonEducation_Queue" });
        }
    }
}
