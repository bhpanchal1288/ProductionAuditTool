using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.VendorRepository
{
    public interface IVendorRepository
    {
        Task<Vendor> GetByIdAsync(int vendorId);
        Task<ITMessage> InsertAsync(Vendor vendor);
        Task<ITMessage> UpdateAsync(Vendor vendor);
        Task<ITMessage> DeleteAsync(Vendor vendor);
    }
}
