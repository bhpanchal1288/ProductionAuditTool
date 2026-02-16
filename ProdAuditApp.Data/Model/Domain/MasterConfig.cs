using System.ComponentModel.DataAnnotations;

namespace ProdAuditApp.Data.Model.Domain;
public class MasterDefinition
{
    public int MasterId { get; set; }
    public string MasterCode { get; set; }
    public string TableName { get; set; }
    public string DisplayName { get; set; }
    public bool IsActive { get; set; }
    public string SelectQuery { get; set; }

    //public string MasterKey { get; set; }
    //public int GroupId { get; set; }
    //public string Title { get; set; }
    //public string ApiUrl { get; set; }
    //public bool AllowView { get; set; }
    //public bool AllowAdd { get; set; }
    //public bool AllowEdit { get; set; }
    //public bool AllowDelete { get; set; }
    //public bool AllowPrint { get; set; }
    //public bool AllowImport { get; set; }
    //public bool AllowExport { get; set; }
    //public bool SelectProc { get; set; }        
    //public List<MasterColumn> Columns { get; set; }

}

public class MasterColumnDefinition
{
    public int ColumnId { get; set; }
    public int MasterId { get; set; } 
    public string ColumnName { get; set; }
    public string DisplayName { get; set; }
    public string DataType { get; set; }
    public bool IsVisible { get; set; }
    public bool IsSortable { get; set; }
    public bool IsFilterable { get; set; }
    public int DisplayOrder { get; set; }


//public string FieldName { get; set; }
//    public string HeaderText { get; set; }
//    public string DataType { get; set; }
//    public bool IsVisible { get; set; }
//    public int DisplayOrder { get; set; }
}

public class MasterDataResult
{
    public IEnumerable<MasterColumnDefinition> Columns { get; set; }
    public IEnumerable<IDictionary<string, object>> Data { get; set; }
    public int TotalRecords { get; set; }
}