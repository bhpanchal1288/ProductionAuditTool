using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.LocationBudgetRepository
{
    public interface ILocationBudgetRepository
    {
        Task<List<LocationBudget>> GetByIdAsync(int projectId);
        Task<IEnumerable<DropdownConfig>> GetLocationTypeDropdownItemsAsync();
        Task<ITMessage> InsertAsync(LocationBudget locationbudget);
        Task<ITMessage> UpdateAsync(LocationBudget locationbudget);
        Task<ITMessage> DeleteAsync(LocationBudget locationbudget);

    }
}
