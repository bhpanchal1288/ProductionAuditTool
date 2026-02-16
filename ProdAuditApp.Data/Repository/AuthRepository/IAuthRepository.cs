using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.AuthRepository
{
    public interface IAuthRepository
    {
        Task<User> LoginAsync(string loginId, string loginPassword);

        Task<List<Menu>> AuthMenuAsync(int groupId);
    }
}
