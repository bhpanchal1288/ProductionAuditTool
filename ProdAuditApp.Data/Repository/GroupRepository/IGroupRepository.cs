using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.GroupRepository
{
    public interface IGroupRepository
    {
        Task<Group> GetByIdAsync(int groupId);
        //Task<IEnumerable<Group>> GetByMasterCodeAsync(string masterCode);
        Task<ITMessage> InsertAsync(Group group);
        Task<ITMessage> UpdateAsync(Group group);
        Task<ITMessage> DeleteAsync(Group group);
    }
}
