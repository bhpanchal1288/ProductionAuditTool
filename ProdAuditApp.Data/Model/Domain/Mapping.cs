namespace ProdAuditApp.Data.Model.Domain
{
    public class Mapping
    {
        public int? mappingid { get; set; }
		public int? projectid { get; set; }
        public string? projectname { get; set; }
        public int groupid { get; set; }
        public string? groupname { get; set; }
        public int? subgroupcategoryid { get; set; }
        public string? category { get; set; }
		public int? subgroupid { get; set; }
        public string? subgroupname { get; set; }
        public string? particulars { get; set; }
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
