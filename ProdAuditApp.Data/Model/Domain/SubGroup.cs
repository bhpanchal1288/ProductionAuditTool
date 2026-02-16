using System;

namespace ProdAuditApp.Data.Model.Domain
{
    public class SubGroup
    {
        public int subgroupid { get; set; }
        public int groupid { get; set; }
        public string subgroupcode { get; set; }
        public string subgroupname { get; set; }
        public int categoryid { get; set; }
        public string subgroupdesc { get; set; }
        public int createdby { get; set; }
        public string createddate { get; set; }
        public int updatedby { get; set; }
        public string updateddate { get; set; }

    }
}
