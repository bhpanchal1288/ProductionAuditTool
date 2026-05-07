using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProdAuditApp.Data.Model.Domain
{
    public class Contracts
    {
        public int? contractid { get; set;  }
        public int projectid { get; set; }
        public int vendorid { get; set; }
        public string? vendorname { get; set; }
        public string contractno { get; set; }
        public DateTime contractdate { get; set; }
        public DateTime workstartdate { get; set; }
        public DateTime workenddate { get; set; }
        public decimal netamount { get; set; }
        public decimal tax { get; set; }
        public decimal grossamount { get; set; }
        public string? description { get; set; }
        public int createdby { get; set; }
        public DateTime? createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime? updateddate { get; set; }

        public List<ContractDetails> Details { get; set; } = new();
    }

    public class ContractDetails
    {
        public int contractdetailsid { get; set; }
        public int contractid { get; set; }
        public int groupid { get; set; }
        public string? groupname { get; set; }
        public int categoryid { get; set; }
        public string? category { get; set; }
        public int subgroupid { get; set; }
        public string? subgroupname { get; set; }
        public decimal quantity { get; set; }
        public decimal rate { get; set; }
        public int uomid { get; set; }
        public string? uom { get; set; }
        public decimal amount { get; set; }
        public int createdby { get; set; }
        public DateTime? createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime? updateddate { get; set; }
    }
}
