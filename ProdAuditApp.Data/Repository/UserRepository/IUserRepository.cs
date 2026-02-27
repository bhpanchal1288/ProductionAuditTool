using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.UserRepository
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int groupId);
        Task<IEnumerable<DropdownConfig>> GetUserGroupDropdownItemsAsync();
        Task<ITMessage> InsertAsync(User user);
        Task<ITMessage> UpdateAsync(User user);
        Task<ITMessage> DeleteAsync(User user);
    }
}
