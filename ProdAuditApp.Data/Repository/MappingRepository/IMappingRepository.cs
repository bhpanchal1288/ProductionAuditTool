using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.MappingRepository
{
    public interface IMappingRepository
    {
        Task<List<Mapping>> GetByIdAsync(int projectId);
        Task<ITMessage> InsertAsync(Mapping mapping);
        Task<ITMessage> UpdateAsync(Mapping mapping);
        Task<ITMessage> DeleteAsync(Mapping mapping);
    }
}
