using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ProductionHouseRepository
{
    public interface IProductionHouseRepository
    {
        Task<ProductionHouse> GetByIdAsync(int productionHouseId);
        Task InsertAsync(ProductionHouse productionHouse);
        Task UpdateAsync(ProductionHouse productionHouse);
        Task DeleteAsync(ProductionHouse productionHouse);
    }
}
