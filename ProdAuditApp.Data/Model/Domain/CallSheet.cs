using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Model.Domain
{
    public class CallSheet
    {
        public int? callsheetid { get; set; }
        public DateTime? reportingdate { get; set; }
        public int? projectid { get; set; }
        public string? projectname { get; set; }
        public int? locationid { get; set; }
        public string? locationname { get; set; }
        public int? shiftid { get; set; }
        public string? shifttype { get; set; }
        public string? sunrise { get; set; }
        public string? sunset { get; set; }
        public string? weather { get; set; }
        public string? breakfast { get; set; }
        public string? lunch { get; set; }
        public string? shiftstarttime { get; set; }
        public string? shiftendtime { get; set; }
        public string? rolltime { get; set; }
        public string? calltime { get; set; }
        public int noofscenes { get; set; }
        public string? nearesthospital { get; set; }
        public string? additionalnotes { get; set; }
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
