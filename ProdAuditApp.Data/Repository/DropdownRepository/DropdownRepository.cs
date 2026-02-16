using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.DropdownRepository
{
    public class DropdownRepository : IDropdownRepository
    {
        private readonly ISqlDataAccess _db;
        public DropdownRepository(ISqlDataAccess db) 
        { 
            _db = db;
        }

        public async Task<List<DropdownConfig>> GetDropdownDataAsync(string cmbName, string FormName, string expr1 = null, string expr2 = null, string expr3 = null, string expr4 = null, string expr5 = null)
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
    }
}
