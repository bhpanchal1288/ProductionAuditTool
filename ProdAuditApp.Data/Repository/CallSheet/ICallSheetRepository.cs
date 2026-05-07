using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.CallSheetRepository
{
    public interface ICallSheetRepository
    {
        //Task<List<CallSheet>> GetCallSheetByIdAsync(int callSheetId);
        Task<CallSheet> GetCallSheetByIdAsync(int callSheetId);
        Task<ITMessage> InsertAsync(CallSheet callSheet);
        Task<ITMessage> UpdateAsync(CallSheet callSheet);
        Task<ITMessage> DeleteAsync(CallSheet callSheet);
    }
}
