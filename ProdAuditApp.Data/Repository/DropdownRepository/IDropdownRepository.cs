using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.DropdownRepository
{
    public interface IDropdownRepository
    {
        Task<List<DropdownConfig>> GetDropdownDataAsync(string cmbName
                                                , string FormName
                                                , string expr1 = null
                                                , string expr2 = null
                                                , string expr3 = null
                                                , string expr4 = null
                                                , string expr5 = null);
    }
}
