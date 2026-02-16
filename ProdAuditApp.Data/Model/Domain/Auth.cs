using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain;

public class Auth
{
    [Required]
    public string username { get; set; }
    [Required]
    public string password { get; set; }
}
