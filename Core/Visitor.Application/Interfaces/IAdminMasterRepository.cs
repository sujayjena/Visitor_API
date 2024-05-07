using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IAdminMasterRepository
    {
        #region Gender
        Task<int> SaveGender(Gender_Request parameters);

        Task<IEnumerable<Gender_Response>> GetGenderList(BaseSearchEntity parameters);

        Task<Gender_Response?> GetGenderById(int Id);
        #endregion

        #region Visitor Type
        Task<int> SaveVisitorType(VisitorType_Request parameters);

        Task<IEnumerable<VisitorType_Response>> GetVisitorTypeList(BaseSearchEntity parameters);

        Task<VisitorType_Response?> GetVisitorTypeById(int Id);
        #endregion

        #region Visit Type
        Task<int> SaveVisitType(VisitType_Request parameters);

        Task<IEnumerable<VisitType_Response>> GetVisitTypeList(BaseSearchEntity parameters);

        Task<VisitType_Response?> GetVisitTypeById(int Id);
        #endregion

        #region Vehicle Type
        Task<int> SaveVehicleType(VehicleType_Request parameters);

        Task<IEnumerable<VehicleType_Response>> GetVehicleTypeList(BaseSearchEntity parameters);

        Task<VehicleType_Response?> GetVehicleTypeById(int Id);
        #endregion

        #region Material With Visitor
        Task<int> SaveMaterialWithVisitor(MaterialWithVisitor_Request parameters);

        Task<IEnumerable<MaterialWithVisitor_Response>> GetMaterialWithVisitorList(BaseSearchEntity parameters);

        Task<MaterialWithVisitor_Response?> GetMaterialWithVisitorById(int Id);
        #endregion

        #region Meeting Location
        Task<int> SaveMeetingLocation(MeetingLocation_Request parameters);

        Task<IEnumerable<MeetingLocation_Response>> GetMeetingLocationList(BaseSearchEntity parameters);

        Task<MeetingLocation_Response?> GetMeetingLocationById(int Id);
        #endregion

        #region Document Type
        Task<int> SaveDocumentType(DocumentType_Request parameters);

        Task<IEnumerable<DocumentType_Response>> GetDocumentTypeList(BaseSearchEntity parameters);

        Task<DocumentType_Response?> GetDocumentTypeById(int Id);
        #endregion

        #region Rejection Reason
        Task<int> SaveRejectionReason(RejectionReason_Request parameters);

        Task<IEnumerable<RejectionReason_Response>> GetRejectionReasonList(BaseSearchEntity parameters);

        Task<RejectionReason_Response?> GetRejectionReasonById(int Id);
        #endregion

        #region UOM
        Task<int> SaveUOM(UOM_Request parameters);

        Task<IEnumerable<UOM_Response>> GetUOMList(BaseSearchEntity parameters);

        Task<UOM_Response?> GetUOMById(int Id);
        #endregion

        #region Contract Type
        Task<int> SaveContractType(ContractType_Request parameters);

        Task<IEnumerable<ContractType_Response>> GetContractTypeList(BaseSearchEntity parameters);

        Task<ContractType_Response?> GetContractTypeById(int Id);
        #endregion

        #region Discipline
        Task<int> SaveDiscipline(Discipline_Request parameters);

        Task<IEnumerable<Discipline_Response>> GetDisciplineList(BaseSearchEntity parameters);

        Task<Discipline_Response?> GetDisciplineById(int Id);
        #endregion
    }
}
