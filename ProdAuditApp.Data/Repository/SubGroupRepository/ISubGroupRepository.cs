using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.SubGroupRepository
{
    public interface ISubGroupRepository
    {
        Task<SubGroup> GetByIdAsync(int groupId);
        //Task<IEnumerable<Group>> GetByMasterCodeAsync(string masterCode);
        Task InsertAsync(SubGroup subGroup);
        Task UpdateAsync(SubGroup subGroup);
        Task DeleteAsync(SubGroup subGroup);
    }
}
