using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.UserGroupRepository
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly ISqlDataAccess _db;

        public UserGroupRepository(ISqlDataAccess db) 
        { 
            _db = db;
        }

        public async Task<UserGroup> GetByIdAsync(int groupId)
        {
            var result = await _db.GetData<UserGroup, dynamic>("usp_Select_UserGroup", new { GroupId = groupId });
            return result.FirstOrDefault();
        }

        public async Task<ITMessage> InsertAsync(UserGroup userGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserGroup",
                            new
                            {
                                userGroup.groupId,
                                userGroup.groupCode,
                                userGroup.groupName,
                                userGroup.groupDesc,
                                userGroup.createdBy,
                                userGroup.updatedBy,
                                RequestType = "Insert"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(UserGroup userGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserGroup",
                            new
                            {
                                userGroup.groupId,
                                userGroup.groupCode,
                                userGroup.groupName,
                                userGroup.groupDesc,
                                userGroup.createdBy,
                                userGroup.updatedBy,
                                RequestType = "Update"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(UserGroup userGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserGroup",
                            new
                            {
                                userGroup.groupId,
                                userGroup.groupCode,
                                userGroup.groupName,
                                userGroup.groupDesc,
                                userGroup.createdBy,
                                userGroup.updatedBy,
                                RequestType = "Delete"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Delete failed"
            };
        }
    }
}
