//using System.Text.RegularExpressions;
using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.GroupRepository
{
    public class GroupRepository : IGroupRepository
    {
        private readonly ISqlDataAccess _db;
        public GroupRepository(ISqlDataAccess db)
        {
            _db = db;
        }
        
        public async Task<Group> GetByIdAsync(int groupId)
        {
            var result = await _db.GetData<Group, dynamic>("usp_Select_Group", new { GroupId = groupId });
            return result.FirstOrDefault();
        }

        //public async Task<Group> GetGroup(int groupId)
        //{
        //    IEnumerable<Group> result = await _db.GetData<Group, dynamic>("usp_Select_Group", new { groupid = groupId });
        //    return result.FirstOrDefault();
        //}

        public async Task<ITMessage> InsertAsync(Group group)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Group",
                            new
                            {
                                group.groupId,
                                group.groupCode,
                                group.groupName,
                                group.groupDesc,
                                group.createdBy,
                                group.updatedBy,
                                RequestType = "Insert"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(Group group)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Group",
                            new
                            {
                                group.groupId,
                                group.groupCode,
                                group.groupName,
                                group.groupDesc,
                                group.createdBy,
                                group.updatedBy,
                                RequestType = "Update"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(Group group)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Group",
                            new
                            {
                                group.groupId,
                                group.groupCode,
                                group.groupName,
                                group.groupDesc,
                                group.createdBy,
                                group.updatedBy,
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
