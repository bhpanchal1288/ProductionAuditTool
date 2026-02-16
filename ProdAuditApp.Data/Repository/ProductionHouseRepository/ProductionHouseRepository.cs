using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ProductionHouseRepository
{
    public class ProductionHouseRepository : IProductionHouseRepository
    {
        private readonly ISqlDataAccess _db;

        public ProductionHouseRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<ProductionHouse> GetByIdAsync(int phId)
        {
            var result = await _db.GetData<ProductionHouse, dynamic>("usp_Select_ProductionHouse", new { phid = phId });
            return result.FirstOrDefault();
        }

        public async Task InsertAsync(ProductionHouse productionHouse)
        {
            await _db.Save("usp_DML_ProductionHouse",
                            new
                            {
                                productionHouse.phId,
                                productionHouse.phCode,
                                productionHouse.phName,
                                productionHouse.contactPerson,
                                productionHouse.address,
                                productionHouse.contact1,
                                productionHouse.contact2,
                                productionHouse.emailId,
                                productionHouse.createdBy,
                                productionHouse.updatedBy,
                                RequestType = "Insert"
                            });
        }

        public async Task UpdateAsync(ProductionHouse productionHouse)
        {
            await _db.Save("usp_DML_ProductionHouse",
                            new
                            {
                                productionHouse.phId,
                                productionHouse.phCode,
                                productionHouse.phName,
                                productionHouse.contactPerson,
                                productionHouse.address,
                                productionHouse.contact1,
                                productionHouse.contact2,
                                productionHouse.emailId,
                                productionHouse.createdBy,
                                productionHouse.updatedBy,
                                RequestType = "Update"
                            });
        }

        public async Task DeleteAsync(ProductionHouse productionHouse)
        {
            await _db.Save("usp_DML_ProductionHouse",
                            new
                            {
                                productionHouse.phId,
                                productionHouse.phCode,
                                productionHouse.phName,
                                productionHouse.contactPerson,
                                productionHouse.address,
                                productionHouse.contact1,
                                productionHouse.contact2,
                                productionHouse.emailId,
                                productionHouse.createdBy,
                                productionHouse.updatedBy,
                                RequestType = "Delete"
                            });
        }
    }
}
