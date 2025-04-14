using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Application.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Persistence.Repositories
{
    public class UserRepository : GenericRepository, IUserRepository
    {
        private IConfiguration _configuration;

        public UserRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        #region User 

        public async Task<int> SaveUser(User_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            //queryParameters.Add("@UserCode", parameters.UserCode);
            queryParameters.Add("@UserName", parameters.UserName);
            queryParameters.Add("@MobileNumber", parameters.MobileNumber);
            queryParameters.Add("@EmailId", parameters.EmailId);
            queryParameters.Add("@Password", !string.IsNullOrWhiteSpace(parameters.Password) ? EncryptDecryptHelper.EncryptString(parameters.Password) : string.Empty);
            queryParameters.Add("@UserType", parameters.UserType);
            queryParameters.Add("@RoleId", parameters.RoleId);
            queryParameters.Add("@ReportingTo", parameters.ReportingTo);
            queryParameters.Add("@CompanyId", parameters.CompanyId);
            //queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@DateOfBirth", parameters.DateOfBirth);
            queryParameters.Add("@DateOfJoining", parameters.DateOfJoining);
            queryParameters.Add("@MobileUniqueId", parameters.MobileUniqId);
            queryParameters.Add("@BloodGroupId", parameters.BloodGroupId);
            queryParameters.Add("@GenderId", parameters.GenderId);
            queryParameters.Add("@MaritalStatusId", parameters.MaritalStatusId);

            queryParameters.Add("@AddressLine", parameters.AddressLine);
            queryParameters.Add("@CountryId", parameters.CountryId);
            queryParameters.Add("@StateId", parameters.StateId);
            queryParameters.Add("@DistrictId", parameters.DistrictId);
            queryParameters.Add("@CityId", parameters.CityId);
            queryParameters.Add("@AreaId", parameters.AreaId);
            queryParameters.Add("@Pincode", parameters.Pincode);

            queryParameters.Add("@IsSameAsPermanent", parameters.IsSameAsPermanent);

            queryParameters.Add("@TemporaryAddress", parameters.TemporaryAddress);
            queryParameters.Add("@Temporary_CountryId", parameters.Temporary_CountryId);
            queryParameters.Add("@Temporary_StateId", parameters.Temporary_StateId);
            queryParameters.Add("@Temporary_DistrictId", parameters.Temporary_DistrictId);
            queryParameters.Add("@Temporary_CityId", parameters.Temporary_CityId);
            queryParameters.Add("@Temporary_Pincode", parameters.Temporary_Pincode);

            queryParameters.Add("@EmergencyName", parameters.EmergencyName);
            queryParameters.Add("@EmergencyContactNumber", parameters.EmergencyContactNumber);
            queryParameters.Add("@EmergencyRelation", parameters.EmergencyRelation);
           
            queryParameters.Add("@AadharNumber", parameters.AadharNumber);
            queryParameters.Add("@AadharOriginalFileName", parameters.AadharOriginalFileName);
            queryParameters.Add("@AadharImage", parameters.AadharImage);
            queryParameters.Add("@PanNumber", parameters.PanNumber);
            queryParameters.Add("@PanCardOriginalFileName", parameters.PanCardOriginalFileName);
            queryParameters.Add("@PanCardImage", parameters.PanCardImage);
            queryParameters.Add("@ProfileOriginalFileName", parameters.ProfileOriginalFileName);
            queryParameters.Add("@ProfileImage", parameters.ProfileImage);
            queryParameters.Add("@OtherProof", parameters.OtherProof);
            queryParameters.Add("@OtherProofOriginalFileName", parameters.OtherProofOriginalFileName);
            queryParameters.Add("@OtherProofImage", parameters.OtherProofImage);
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@IsHOD", parameters.IsHOD);
            queryParameters.Add("@IsManager", parameters.IsManager);
            queryParameters.Add("@IsMobileUser", parameters.IsMobileUser);
            queryParameters.Add("@IsWebUser", parameters.IsWebUser);
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUser", queryParameters);
        }

        public async Task<IEnumerable<User_Response>> GetUserList(User_Search parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@BranchId", parameters.BranchId);
            queryParameters.Add("@DepartmentId", parameters.DepartmentId);
            queryParameters.Add("@SearchText", parameters.SearchText.SanitizeValue());
            queryParameters.Add("@IsActive", parameters.IsActive);
            queryParameters.Add("@PageNo", parameters.PageNo);
            queryParameters.Add("@PageSize", parameters.PageSize);
            queryParameters.Add("@Total", parameters.Total, null, System.Data.ParameterDirection.Output);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            var result = await ListByStoredProcedure<User_Response>("GetUserList", queryParameters);

            if(SessionManager.LoggedInUserId > 1)
            {
                if (SessionManager.LoggedInUserId > 2)
                {
                    result = result.Where(x => x.Id > 2).ToList();
                }
                else
                {
                    result = result.Where(x => x.Id > 1).ToList();
                }
            }

            parameters.Total = queryParameters.Get<int>("Total");

            return result;
        }

        public async Task<User_Response?> GetUserById(long Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<User_Response>("GetUserById", queryParameters)).FirstOrDefault();
        }

        #endregion

        #region User Other Details
        public async Task<int> SaveUserOtherDetails(UserOtherDetails_Request parameters)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", parameters.Id);
            queryParameters.Add("@EmployeeId", parameters.EmployeeId);
            queryParameters.Add("@PastCompanyName", parameters.PastCompanyName);
            queryParameters.Add("@TotalExp", parameters.TotalExp);
            queryParameters.Add("@Remark", parameters.Remark);
            queryParameters.Add("@UserId", SessionManager.LoggedInUserId);

            return await SaveByStoredProcedure<int>("SaveUserOtherDetails", queryParameters);
        }

        public async Task<IEnumerable<UserOtherDetails_Response>> GetUserOtherDetailsByEmployeeId(int EmployeeId)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@EmployeeId", EmployeeId);

            var result = await ListByStoredProcedure<UserOtherDetails_Response>("GetUserOtherDetailsByEmployeeId", queryParameters);

            return result;
        }

        public async Task<UserOtherDetails_Response?> GetUserOtherDetailsById(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();
            queryParameters.Add("@Id", Id);
            return (await ListByStoredProcedure<UserOtherDetails_Response>("GetUserOtherDetailsById", queryParameters)).FirstOrDefault();
        }


        public async Task<int> DeleteUserOtherDetails(int Id)
        {
            DynamicParameters queryParameters = new DynamicParameters();

            queryParameters.Add("@Id", Id);

            return await SaveByStoredProcedure<int>("DeleteUserOtherDetails", queryParameters);
        }
        #endregion
    }
}
