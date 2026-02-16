
namespace ProdAuditApp.Data.Model.Domain;
public class Menu
{
    public int userauthid { get; set; }
    public int groupid { get; set; }
    public int menuid { get; set; }
    public string menuname { get; set; }
    public string filtermenuname { get; set; }
    public bool isparent { get; set; }
    public int parentmenuid { get; set; }
    public string pagename { get; set; }
    public int menuorder { get; set; }
    public bool isview { get; set; }
    public bool isadd { get; set; }
    public bool isupdate { get; set; }
    public bool isdelete { get; set; }
    public bool isprint { get; set; }
    public bool isimport { get; set; }
    public bool isexport { get; set; }
    public string selectproc { get; set; }
}

