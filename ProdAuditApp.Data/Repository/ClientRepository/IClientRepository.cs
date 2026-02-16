using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ClientRepository
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int clientId);
        Task InsertAsync(Client client);
        Task UpdateAsync(Client client);
        Task DeleteAsync(Client client);
    }
}
