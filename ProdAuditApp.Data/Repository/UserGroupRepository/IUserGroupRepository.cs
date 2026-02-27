using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.UserGroupRepository
{
    public interface IUserGroupRepository
    {
        Task<UserGroup> GetByIdAsync(int groupId);
        Task<ITMessage> InsertAsync(UserGroup userGroup);
        Task<ITMessage> UpdateAsync(UserGroup userGroup);
        Task<ITMessage> DeleteAsync(UserGroup userGroup);
    }
}
