using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.CallSheetRepository;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProdAuditApp.Data.Repository.CallSheetRepository
{
    public class CallSheetRepository : ICallSheetRepository
    {
        private readonly ISqlDataAccess _db;
        public CallSheetRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<CallSheet> GetCallSheetByIdAsync(int callSheetId)
        {
            var result = await _db.GetData<CallSheet, dynamic>("usp_Select_CallSheet", new { callsheetid = callSheetId });
            return result.FirstOrDefault();
        }

        public async Task<ITMessage> InsertAsync(CallSheet callSheet)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_CallSheet",
                new
                {                    
                    callSheet.callsheetid,
                    callSheet.reportingdate,
                    callSheet.projectid,
                    callSheet.locationid,
                    callSheet.shiftid,
                    callSheet.sunrise,
                    callSheet.sunset,
                    callSheet.weather,
                    callSheet.breakfast,
                    callSheet.lunch,
                    callSheet.shiftstarttime,
                    callSheet.shiftendtime,
                    callSheet.rolltime,
                    callSheet.calltime,
                    callSheet.noofscenes,
                    callSheet.nearesthospital,
                    callSheet.additionalnotes,
                    callSheet.createdBy,
                    callSheet.updatedBy,
                    RequestType = "Insert"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Insert failed" };
        }

        public async Task<ITMessage> UpdateAsync(CallSheet callSheet)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_CallSheet",
                new
                {
                    callSheet.callsheetid,
                    callSheet.reportingdate,
                    callSheet.projectid,
                    callSheet.locationid,
                    callSheet.shiftid,
                    callSheet.sunrise,
                    callSheet.sunset,
                    callSheet.weather,
                    callSheet.breakfast,
                    callSheet.lunch,
                    callSheet.shiftstarttime,
                    callSheet.shiftendtime,
                    callSheet.rolltime,
                    callSheet.calltime,
                    callSheet.noofscenes,
                    callSheet.nearesthospital,
                    callSheet.additionalnotes,
                    callSheet.createdBy,
                    callSheet.updatedBy,
                    RequestType = "Update"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Update failed" };
        }

        public async Task<ITMessage> DeleteAsync(CallSheet callSheet)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_CallSheet",
                new
                {
                    callSheet.callsheetid,
                    callSheet.reportingdate,
                    callSheet.projectid,
                    callSheet.locationid,
                    callSheet.shiftid,
                    callSheet.sunrise,
                    callSheet.sunset,
                    callSheet.weather,
                    callSheet.breakfast,
                    callSheet.lunch,
                    callSheet.shiftstarttime,
                    callSheet.shiftendtime,
                    callSheet.rolltime,
                    callSheet.calltime,
                    callSheet.noofscenes,
                    callSheet.nearesthospital,
                    callSheet.additionalnotes,
                    callSheet.createdBy,
                    callSheet.updatedBy,
                    RequestType = "Delete"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Delete failed" };
        }
    }
}
