using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.SubGroupRepository
{
    public interface ISubGroupRepository
    {
        Task<SubGroup> GetByIdAsync(int groupId);
        //Task<IEnumerable<Group>> GetByMasterCodeAsync(string masterCode);
        Task<IEnumerable<DropdownConfig>> GetGroupDropdownItemsAsync();
        Task<IEnumerable<DropdownConfig>> GetCategoryDropdownItemsAsync();
        Task<IEnumerable<DropdownConfig>> GetSubGroupDropdownItemsAsync(int groupId, int categoryId);
        Task<ITMessage> InsertAsync(SubGroup subGroup);
        Task<ITMessage> UpdateAsync(SubGroup subGroup);
        Task<ITMessage> DeleteAsync(SubGroup subGroup);
    }
}
