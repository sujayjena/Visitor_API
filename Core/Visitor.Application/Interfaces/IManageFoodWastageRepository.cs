using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;
using Visitor.Persistence.Repositories;

namespace Visitor.Application.Interfaces
{
    public interface IManageFoodWastageRepository
    {
        Task<int> SaveFoodWastage(FoodWastage_Request parameters);

        Task<IEnumerable<FoodWastage_Response>> GetFoodWastageList(FoodWastage_Search parameters);

        Task<FoodWastage_Response?> GetFoodWastageById(int Id);
    }
}
