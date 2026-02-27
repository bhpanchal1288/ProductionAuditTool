using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain
{
    public class User
    {
        [Required]
        public int? userid { get; set; }
        [Required]
        public int groupid { get; set; }
        [Required]
        public string firstname { get; set; }
        [Required]
        public string lastname { get; set; }
        [Required]
        public decimal mobile { get; set; }
        [Required]
        public string emailid { get; set; }
        [Required]
        public string password { get; set; }
        public int createdby { get; set; }
        public DateTime createddate { get; set; }
        public int updatedby { get; set; }
        public DateTime updateddate { get; set; }

    }
}