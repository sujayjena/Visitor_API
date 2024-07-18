using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visitor.Application.Models;

namespace Visitor.Application.Interfaces
{
    public interface IManageWorkerRepository
    {
        Task<int> SaveWorker(Worker_Request parameters);

        Task<IEnumerable<Worker_Response>> GetWorkerList(WorkerSearch_Request parameters);

        Task<Worker_Response?> GetWorkerById(int Id);
    }
}
