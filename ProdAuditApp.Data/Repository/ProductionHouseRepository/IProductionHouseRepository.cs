using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ProductionHouseRepository
{
    public interface IProductionHouseRepository
    {
        Task<ProductionHouse> GetByIdAsync(int productionHouseId);
        Task<ITMessage> InsertAsync(ProductionHouse productionHouse);
        Task<ITMessage> UpdateAsync(ProductionHouse productionHouse);
        Task<ITMessage> DeleteAsync(ProductionHouse productionHouse);
    }
}
