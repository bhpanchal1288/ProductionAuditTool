using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.VendorRepository
{
    public class VendorRepository : IVendorRepository
    {
        private readonly ISqlDataAccess _db;

        public VendorRepository(ISqlDataAccess db)
        {
            _db = db;
        }
        public async Task<Vendor> GetByIdAsync(int vendorId)
        {
            var result = await _db.GetData<Vendor, dynamic>("usp_Select_Vendor", new { VendorId = vendorId });
            return result.FirstOrDefault();
        }

        public async Task InsertAsync(Vendor vendor)
        {
            await _db.Save("usp_DML_Vendor",
                            new
                            {
                                vendor.vendorId,
                                vendor.vendorCode,
                                vendor.vendorName,
                                vendor.contactPerson,
                                vendor.address,
                                vendor.contact1,
                                vendor.contact2,
                                vendor.emailId,
                                vendor.createdBy,
                                vendor.updatedBy,
                                RequestType = "Insert"
                            });
        }

        public async Task UpdateAsync(Vendor vendor)
        {
            await _db.Save("usp_DML_Vendor",
                            new
                            {
                                vendor.vendorId,
                                vendor.vendorCode,
                                vendor.vendorName,
                                vendor.contactPerson,
                                vendor.address,
                                vendor.contact1,
                                vendor.contact2,
                                vendor.emailId,
                                vendor.createdBy,
                                vendor.updatedBy,
                                RequestType = "Update"
                            });
        }

        public async Task DeleteAsync(Vendor vendor)
        {
            await _db.Save("usp_DML_Vendor",
                            new
                            {
                                vendor.vendorId,
                                vendor.vendorCode,
                                vendor.vendorName,
                                vendor.contactPerson,
                                vendor.address,
                                vendor.contact1,
                                vendor.contact2,
                                vendor.emailId,
                                vendor.createdBy,
                                vendor.updatedBy,
                                RequestType = "Delete"
                            });
        }

    }
}
