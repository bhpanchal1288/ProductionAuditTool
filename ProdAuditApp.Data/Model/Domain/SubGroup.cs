using System;
using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain
{
    public class SubGroup
    {
        public int? subgroupid { get; set; }
        public int groupid { get; set; }

        [Required]
        public string subgroupcode { get; set; }

        [Required]
        public string subgroupname { get; set; }
        public int categoryid { get; set; }
        public string? subgroupdesc { get; set; }
        public int createdby { get; set; }
        public DateTime? createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime? updateddate { get; set; }

    }
}
