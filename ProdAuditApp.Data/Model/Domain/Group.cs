using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdAuditApp.Data.Model.Domain
{
    public class Group
    {
        public int groupId { get; set; }
        public string groupCode { get; set; }
        public string groupName { get; set; }
        public string groupDesc { get; set; }
        public int createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedDate { get; set; }
    }
}
