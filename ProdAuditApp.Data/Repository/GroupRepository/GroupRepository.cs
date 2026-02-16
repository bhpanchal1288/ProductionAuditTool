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

        public async Task InsertAsync(Group group)
        {
            await _db.Save("usp_DML_Group",
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
        }

        public async Task UpdateAsync(Group group)
        {
            await _db.Save("usp_DML_Group",
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
        }

        public async Task DeleteAsync(Group group)
        {
            await _db.Save("usp_DML_Group",
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
        }
    }
}
