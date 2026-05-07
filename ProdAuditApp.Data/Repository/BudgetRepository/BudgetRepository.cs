using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.BudgetRepository
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly ISqlDataAccess _db;
        public BudgetRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<Budget>> GetByIdAsync(int projectId)
        {
            var result = await _db.GetData<Budget, dynamic>("usp_Select_Budget", new { projectId = projectId });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(Budget budget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Budget",
                new
                {
                    budget.budgetid,
                    budget.mappingid,
                    budget.projectid,
                    budget.quantity,
                    budget.budgetcost,
                    budget.createdBy,
                    budget.updatedBy,
                    RequestType = "Insert"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Insert failed" };
        }

        public async Task<ITMessage> UpdateAsync(Budget budget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Budget",
                new
                {
                    budget.budgetid,
                    budget.mappingid,
                    budget.projectid,
                    budget.quantity,
                    budget.budgetcost,
                    budget.createdBy,
                    budget.updatedBy,
                    RequestType = "Update"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Update failed" };
        }

        public async Task<ITMessage> DeleteAsync(Budget budget)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Budget",
                new
                {
                    budget.budgetid,
                    budget.mappingid,
                    budget.projectid,
                    budget.quantity,
                    budget.budgetcost,
                    budget.createdBy,
                    budget.updatedBy,
                    RequestType = "Delete"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Delete failed" };
        }

    }
}
