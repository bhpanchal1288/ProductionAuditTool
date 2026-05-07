using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Repository.ContractsRepository
{
    public class ContractsRepository : IContractsRepository
    {
        private readonly ISqlDataAccess _db;
        public ContractsRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<Contracts>> GetByContractAsync(int contractId)
        {
            var result = await _db.GetData<Contracts, dynamic>("usp_Select_Contracts", new { contractid = contractId });
            return result.ToList();
        }

        public async Task<List<ContractDetails>> GetDetailsByContractIdAsync(int contractId)
        {
            var result = await _db.GetData<ContractDetails, dynamic>("usp_Select_ContractDetails", new { contractId = contractId });
            return result.ToList();
        }

        private DataTable ToDataTable(List<ContractDetails> items)
        {
            var table = new DataTable();

            //table.Columns.Add("contractid", typeof(int));
            table.Columns.Add("gorupid", typeof(int));
            table.Columns.Add("categoryid", typeof(int));
            table.Columns.Add("subgroupid", typeof(int));
            table.Columns.Add("quantity", typeof(decimal));
            table.Columns.Add("rate", typeof(decimal));
            table.Columns.Add("uom", typeof(int));
            table.Columns.Add("amount", typeof(decimal));

            foreach (var item in items)
            {
                table.Rows.Add(
                    //item.contractid,
                    item.groupid,
                    item.categoryid,
                    item.subgroupid,
                    item.quantity,
                    item.rate,
                    item.uom,
                    item.amount
                );
            }

            return table;
        }

        public async Task<ITMessage> InsertAsync(Contracts contract)
        {
            //var contractdetails = ToDataTable(contract.Details);

            // Save header
            var res = await _db.SaveData<ITMessage>("usp_DML_Contracts",
                new
                {
                    contract.contractid,
                    contract.projectid,
                    contract.vendorid,
                    contract.contractno,
                    contract.contractdate,
                    contract.workstartdate,
                    contract.workenddate,
                    contract.netamount,
                    contract.tax,
                    contract.grossamount,
                    contract.description,
                    contract.createdby,
                    contract.updatedby,
                    RequestType = "Insert"
                });

            var resDelete = await _db.SaveData<ITMessage>("usp_DML_Contracts",
                new
                {
                    contract.contractid,
                    contract.projectid,
                    contract.vendorid,
                    contract.contractno,
                    contract.contractdate,
                    contract.workstartdate,
                    contract.workenddate,
                    contract.netamount,
                    contract.tax,
                    contract.grossamount,
                    contract.description,
                    contract.createdby,
                    contract.updatedby,
                    RequestType = "DELETEDETAILS"
                });

            // Optionally save details if header inserted successfully - assumes stored procedure returns identity
            // Not implementing details save here; controller may handle details separately or a combined SP can be used.

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Insert failed" };
        }

        public async Task<ITMessage> SaveDetailAsync(ContractDetails detail, string requestType)
        {

            var res = await _db.SaveData<ITMessage>("usp_DML_ContractDetails",
                new
                {
                    detail.contractdetailsid,
                    detail.contractid,
                    detail.groupid,
                    detail.categoryid,
                    detail.subgroupid,
                    detail.quantity,
                    detail.rate,
                    detail.uom,
                    detail.amount,
                    detail.createdby,
                    detail.updatedby,
                    RequestType = requestType
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Detail save failed" };
        }

        public async Task<ITMessage> UpdateAsync(Contracts contract)
        {
            var contractdetails = ToDataTable(contract.Details);

            var res = await _db.SaveData<ITMessage>("usp_DML_Contracts",
                new
                {
                    contract.contractid,
                    contract.projectid,
                    contract.vendorid,
                    contract.contractno,
                    contract.contractdate,
                    contract.workstartdate,
                    contract.workenddate,
                    contract.netamount,
                    contract.tax,
                    contract.grossamount,
                    contract.description,
                    contract.createdby,
                    contract.updatedby,
                    RequestType = "Update"
                });

            var resDelete = await _db.SaveData<ITMessage>("usp_DML_Contracts",
                new
                {
                    contract.contractid,
                    contract.projectid,
                    contract.vendorid,
                    contract.contractno,
                    contract.contractdate,
                    contract.workstartdate,
                    contract.workenddate,
                    contract.netamount,
                    contract.tax,
                    contract.grossamount,
                    contract.description,
                    contract.createdby,
                    contract.updatedby,
                    RequestType = "DELETEDETAILS"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Update failed" };
        }

        public async Task<ITMessage> DeleteAsync(Contracts contract)
        {
            var contractdetails = ToDataTable(contract.Details);
            var res = await _db.SaveData<ITMessage>("usp_DML_Contracts",
                new
                {
                    contract.contractid,
                    contract.projectid,
                    contract.vendorid,
                    contract.contractno,
                    contract.contractdate,
                    contract.workstartdate,
                    contract.workenddate,
                    contract.netamount,
                    contract.tax,
                    contract.grossamount,
                    contract.description,
                    contract.createdby,
                    contract.updatedby,
                    RequestType = "Delete"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Delete failed" };
        }
    }
}
