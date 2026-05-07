using ProdAuditApp.Data.Model.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.ContractsRepository
{
    public interface IContractsRepository
    {
        Task<List<Contracts>> GetByContractAsync(int contractId);
        Task<List<ContractDetails>> GetDetailsByContractIdAsync(int contractId);
        Task<ITMessage> InsertAsync(Contracts contract);
        Task<ITMessage> UpdateAsync(Contracts contract);
        Task<ITMessage> DeleteAsync(Contracts contract);
    }
}
