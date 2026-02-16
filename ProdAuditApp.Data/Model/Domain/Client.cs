namespace ProdAuditApp.Data.Model.Domain
{
    public class Client
    {
        public int clientId { get; set; }
        public string clientCode { get; set; }
        public string clientName { get; set; }
        public string contactPerson { get; set; }
        public string address { get; set; }
        public decimal contact1 { get; set; }
		public decimal? contact2 { get; set; }
        public string? emailId { get; set; }
        public int createdBy { get; set; }
        public DateTime createdDate { get; set; }
        public int updatedBy { get; set; }
        public DateTime updatedDate { get; set; }
    }
}
