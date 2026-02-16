using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain;
public class User
{
    public int userid { get; set; }
    public int groupid { get; set; }
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string stafftype { get; set; }
    public int designation { get; set; }
    public decimal mobile { get; set; }
    public string emailid { get; set; }
    public string password { get; set; }
    public int createdby { get; set; }
    public int updatedby { get; set; }

}