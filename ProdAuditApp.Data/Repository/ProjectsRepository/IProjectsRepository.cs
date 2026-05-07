using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.ProjectsRepository
{
    public interface IProjectsRepository
    {
        Task<Projects> GetByIdAsync(int projectId);
        Task<IEnumerable<DropdownConfig>> GetClientDropdownItemsAsync();
        Task<IEnumerable<DropdownConfig>> GetProductionHouseDropdownItemsAsync();
        Task<IEnumerable<DropdownConfig>> GetProjectTypeDropdownItemsAsync();
        Task<ITMessage> InsertAsync(Projects project);
        Task<ITMessage> UpdateAsync(Projects project);
        Task<ITMessage> DeleteAsync(Projects project);

    }
}
