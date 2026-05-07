using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.SubGroupRepository
{
    public class SubGroupRepository : ISubGroupRepository
    {
        private readonly ISqlDataAccess _db;
        public SubGroupRepository(ISqlDataAccess db) 
        { 
            _db = db; 
        }

        public async Task<SubGroup> GetByIdAsync(int subGroupId)
        {
            var result = await _db.GetData<SubGroup, dynamic>("usp_Select_SubGroup", new { subgroupid = subGroupId });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<DropdownConfig>> GetGroupDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "GROUP",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<IEnumerable<DropdownConfig>> GetCategoryDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "SubGroupCategory",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<IEnumerable<DropdownConfig>> GetSubGroupDropdownItemsAsync(int groupId, int categoryId)
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "SubGroup",
                    FormName = "BLANK",
                    expr1 = groupId,
                    expr2 = categoryId,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(SubGroup subGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_SubGroup",
                            new
                            {
                                subGroup.subgroupid,
                                subGroup.groupid,
                                subGroup.subgroupcode,
                                subGroup.subgroupname,
                                subGroup.categoryid,
                                subGroup.subgroupdesc,
                                subGroup.createdby,
                                subGroup.updatedby,
                                RequestType = "Insert"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(SubGroup subGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_SubGroup",
                            new
                            {
                                subGroup.subgroupid,
                                subGroup.groupid,
                                subGroup.subgroupcode,
                                subGroup.subgroupname,
                                subGroup.categoryid,
                                subGroup.subgroupdesc,
                                subGroup.createdby,
                                subGroup.updatedby,
                                RequestType = "Update"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(SubGroup subGroup)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_SubGroup",
                            new
                            {
                                subGroup.subgroupid,
                                subGroup.groupid,
                                subGroup.subgroupcode,
                                subGroup.subgroupname,
                                subGroup.categoryid,
                                subGroup.subgroupdesc,
                                subGroup.createdby,
                                subGroup.updatedby,
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
