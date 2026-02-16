using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.VendorRepository
{
    public interface IVendorRepository
    {
        Task<Vendor> GetByIdAsync(int vendorId);
        Task InsertAsync(Vendor vendor);
        Task UpdateAsync(Vendor vendor);
        Task DeleteAsync(Vendor vendor);
    }
}
