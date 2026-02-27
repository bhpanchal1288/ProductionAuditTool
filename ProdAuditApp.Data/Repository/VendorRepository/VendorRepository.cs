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

        public async Task<ITMessage> InsertAsync(Vendor vendor)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Vendor",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(Vendor vendor)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Vendor",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(Vendor vendor)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Vendor",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Delete failed"
            };
        }

    }
}
