using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.DropdownRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.SubGroupRepository
{
    public class SubGroupRepository : ISubGroupRepository
    {
        private readonly ISqlDataAccess _db;
        private readonly IDropdownRepository _dropdown;
        public SubGroupRepository(ISqlDataAccess db, IDropdownRepository dropdown) 
        { 
            _db = db; 
            _dropdown = dropdown;
        }

        public async Task<SubGroup> GetByIdAsync(int subGroupId)
        {
            var result = await _db.GetData<SubGroup, dynamic>("usp_Select_SubGroup", new { subgroupid = subGroupId });
            return result.FirstOrDefault();
        }

        public async Task<List<DropdownConfig>> GetDropdownItemsAsync(string cmbName, string FormName, string expr1, string expr2, string expr3, string expr4, string expr5)
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new {
                    cmbName = cmbName,
                    FormName = FormName,
                    expr1 = expr1,
                    expr2 = expr2,
                    expr3 = expr3,
                    expr4 = expr4,
                    expr5 = expr5
                });
            return result.ToList();
        }

        public async Task InsertAsync(SubGroup subGroup)
        {
            await _db.Save("usp_DML_SubGroup",
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
        }

        public async Task UpdateAsync(SubGroup subGroup)
        {
            await _db.Save("usp_DML_SubGroup",
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
        }

        public async Task DeleteAsync(SubGroup subGroup)
        {
            await _db.Save("usp_DML_SubGroup",
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
        }
    }
}
