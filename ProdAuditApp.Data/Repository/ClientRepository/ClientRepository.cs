using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;
using System.Text.RegularExpressions;

namespace ProdAuditApp.Data.Repository.ClientRepository
{
    public class ClientRepository : IClientRepository
    {
        private readonly ISqlDataAccess _db;

        public ClientRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Client> GetByIdAsync(int clientId)
        {
            var result = await _db.GetData<Client, dynamic>("usp_Select_Client", new { ClientId = clientId });
            return result.FirstOrDefault();
        }

        public async Task InsertAsync(Client client)
        {
            await _db.Save("usp_DML_Client",
                            new
                            {
                                client.clientId,
                                client.clientCode,
                                client.clientName,
                                client.contactPerson,
                                client.address,
                                client.contact1,
                                client.contact2,
                                client.emailId,
                                client.createdBy,
                                client.updatedBy,
                                RequestType = "Insert"
                            });
        }

        public async Task UpdateAsync(Client client)
        {
            await _db.Save("usp_DML_Client",
                            new
                            {
                                client.clientId,
                                client.clientCode,
                                client.clientName,
                                client.contactPerson,
                                client.address,
                                client.contact1,
                                client.contact2,
                                client.emailId,
                                client.createdBy,
                                client.updatedBy,
                                RequestType = "Update"
                            });
        }

        public async Task DeleteAsync(Client client)
        {
            await _db.Save("usp_DML_Client",
                            new
                            {
                                client.clientId,
                                client.clientCode,
                                client.clientName,
                                client.contactPerson,
                                client.address,
                                client.contact1,
                                client.contact2,
                                client.emailId,
                                client.createdBy,
                                client.updatedBy,
                                RequestType = "Delete"
                            });
        }
    }
}
