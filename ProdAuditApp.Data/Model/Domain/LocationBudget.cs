namespace ProdAuditApp.Data.Model.Domain
{
    public class LocationBudget
    {
        public int locationbudgetid {  get; set; }
		public int projectid {  get; set; }
        public string? projectname { get; set; }
        public string locationname {  get; set; }
		public int locationtypeid {  get; set; }
		public string? locationtype {  get; set; }
		public string address {  get; set; }
		public int shootdays {  get; set; }
		public decimal budgetcost {  get; set; }
        public int? createdBy { get; set; }
        public DateTime? createdDate { get; set; }
        public int? updatedBy { get; set; }
        public DateTime? updatedDate { get; set; }
    }
}
