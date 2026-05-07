using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.BudgetRepository
{
    public interface IBudgetRepository
    {
        Task<List<Budget>> GetByIdAsync(int projectId);
        Task<ITMessage> InsertAsync(Budget budget);
        Task<ITMessage> UpdateAsync(Budget budget);
        Task<ITMessage> DeleteAsync(Budget budget);
    }
}
