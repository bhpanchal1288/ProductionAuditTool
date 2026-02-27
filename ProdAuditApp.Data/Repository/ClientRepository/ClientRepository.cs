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

        public async Task<ITMessage> InsertAsync(Client client)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Client",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(Client client)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Client",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(Client client)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Client",
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
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Delete failed"
            };
        }
    }
}
