using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain
{
    public class ProductionHouse
    {
        public int? phId { get; set; }

        [Required]
        public string phCode { get; set; }

        [Required]
        public string phName { get; set; }

        [Required]
        public string contactPerson { get; set; }
        public string? address { get; set; }

        [Required]
        public decimal contact1 { get; set; }
        public decimal? contact2 { get; set; }

        [Required]
        public string emailId { get; set; }
        public int createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
