using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.GroupRepository
{
    public interface IGroupRepository
    {
        Task<Group> GetByIdAsync(int groupId);
        //Task<IEnumerable<Group>> GetByMasterCodeAsync(string masterCode);
        Task InsertAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(Group group);
    }
}
