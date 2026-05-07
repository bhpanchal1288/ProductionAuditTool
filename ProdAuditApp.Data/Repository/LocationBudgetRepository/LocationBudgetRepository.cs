using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.LocationBudgetRepository
{
    public class LocationBudgetRepository : ILocationBudgetRepository
    {
        private readonly ISqlDataAccess _db;
        public LocationBudgetRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<LocationBudget>> GetByIdAsync(int projectId)
        {
            var result = await _db.GetData<LocationBudget, dynamic>("usp_Select_LocationBudget", new { projectId = projectId });
            return result.ToList();
        }

        public async Task<IEnumerable<DropdownConfig>> GetLocationTypeDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "LocationType",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(LocationBudget locationbudget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_LocationBudget",
                new
                {
                    locationbudget.locationbudgetid,
                    locationbudget.projectid,
                    locationbudget.locationname,
                    locationbudget.locationtypeid,
                    locationbudget.address,
                    locationbudget.shootdays,
                    locationbudget.budgetcost,
                    locationbudget.createdBy,
                    locationbudget.updatedBy,
                    RequestType = "Insert"


                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Insert failed" };
        }

        public async Task<ITMessage> UpdateAsync(LocationBudget locationbudget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_LocationBudget",
                new
                {
                    locationbudget.locationbudgetid,
                    locationbudget.projectid,
                    locationbudget.locationname,
                    locationbudget.locationtypeid,
                    locationbudget.address,
                    locationbudget.shootdays,
                    locationbudget.budgetcost,
                    locationbudget.createdBy,
                    locationbudget.updatedBy,
                    RequestType = "Update"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Update failed" };
        }

        public async Task<ITMessage> DeleteAsync(LocationBudget locationbudget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_LocationBudget",
                new
                {
                    locationbudget.locationbudgetid,
                    locationbudget.projectid,
                    locationbudget.locationname,
                    locationbudget.locationtypeid,
                    locationbudget.address,
                    locationbudget.shootdays,
                    locationbudget.budgetcost,
                    locationbudget.createdBy,
                    locationbudget.updatedBy,
                    RequestType = "Delete"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Delete failed" };
        }
    }
}
