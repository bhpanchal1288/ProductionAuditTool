using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain
{
    public class UserGroup
    {
        public int? groupId { get; set; }

        [Required(ErrorMessage = "Group Code is required")]
        //[StringLength(10, ErrorMessage = "Group Code cannot exceed 10 characters")]
        public string groupCode { get; set; }
        
        [Required(ErrorMessage = "Group Name is required")]
        public string groupName { get; set; }
        public string? groupDesc { get; set; }
        public int createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
