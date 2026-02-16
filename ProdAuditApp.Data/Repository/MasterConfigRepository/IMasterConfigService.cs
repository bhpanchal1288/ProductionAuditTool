using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.MasterConfigRepository
{
    public interface IMasterConfigService
    {
        Task<MasterDataResult> GetDataAsync(
        string masterCode,
        int page,
        int pageSize,
        string sort,
        string dir);
    }
}
