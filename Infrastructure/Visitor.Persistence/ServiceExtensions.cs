using Visitor.Application.Helpers;
using Visitor.Application.Interfaces;
using Visitor.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Visitor.Persistence
{
    public static class ServiceExtensions
    {
        public static void ConfigurePersistence(this IServiceCollection services, IConfiguration configuration)
        {
            //var connectionString = configuration.GetConnectionString("DefaultConnection");
            //services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(connectionString));

            services.AddScoped<IGenericRepository, GenericRepository>();
            services.AddScoped<IJwtUtilsRepository, JwtUtilsRepository>();
            services.AddScoped<IFileManager, FileManager>();

            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<IRolePermissionRepository, RolePermissionRepository>();
            services.AddScoped<ICompanyTypeRepository, CompanyTypeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<ITerritoryRepository, TerritoryRepository>();
            services.AddScoped<IAdminMasterRepository, AdminMasterRepository>();
            services.AddScoped<IManageVisitorCompanyRepository, ManageVisitorCompanyRepository>();
            services.AddScoped<IManageVisitorsRepository, ManageVisitorsRepository>();
            services.AddScoped<IManageContractorRepository, ManageContractorRepository>();
            services.AddScoped<IManageWorkerRepository, ManageWorkerRepository>();
            services.AddScoped<IManageOrderRepository, ManageOrderRepository>();
            services.AddScoped<IManageRFIDRepository, ManageRFIDRepository>();
            services.AddScoped<IManageSecurityRepository, ManageSecurityRepository>();
            services.AddScoped<IManageAttendanceRepository, ManageAttendanceRepository>();
            services.AddScoped<IBarcodeRepository, BarcodeRepository>();
            services.AddScoped<IEmailConfigRepository, EmailConfigRepository>();
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<IConfigRefRepository, ConfigRefRepository>();
            services.AddScoped<IQRCodeRepository, QRCodeRepository>();
            services.AddScoped<IAssignGateNoRepository, AssignGateNoRepository>();
            services.AddScoped<ISMSHelper, SMSHelper>();
            services.AddScoped<ISMSConfigRepository, SMSConfigRepository>();
            services.AddScoped<IManageEarlyLeaveRepository, ManageEarlyLeaveRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            services.AddScoped<IManagePurchaseOrderRepository, ManagePurchaseOrderRepository>();
            services.AddScoped<IManageVendorRepository, ManageVendorRepository>();
            services.AddScoped<ICanteenTransactionRepository, CanteenTransactionRepository>();
        }
    }
}
