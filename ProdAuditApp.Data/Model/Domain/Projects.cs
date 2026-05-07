using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain
{
    public class Projects
    {
        public int? projectId { get; set; }
        [Required]
        public string projectCode { get; set; }
        [Required]
        public string projectName { get; set; }
        [Required]
        public int? clientId { get; set; }
        [Required]
        public int? phId { get; set; }
        [Required]
        public int? projectType { get; set; }
        [Required]
        public DateTime startDate { get; set; }
        [Required]
        public DateTime tentendDate { get; set; }
        public DateTime? endDate { get; set; }
        public int? createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int? updatedBy { get; set; }
        public DateTime updatedDate { get; set; }

    }
}
