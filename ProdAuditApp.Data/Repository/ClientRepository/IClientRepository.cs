using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ClientRepository
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int clientId);
        Task<ITMessage> InsertAsync(Client client);
        Task<ITMessage> UpdateAsync(Client client);
        Task<ITMessage> DeleteAsync(Client client);
    }
}
