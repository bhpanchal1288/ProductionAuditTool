using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.AuthRepository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ISqlDataAccess _db;
        public AuthRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<User> LoginAsync(string loginId, string loginPassword)
        {
            IEnumerable<User> result = await _db.GetData<User, dynamic>("usp_Login", new { username = loginId, password = loginPassword });
            return result.FirstOrDefault();
        }

        public async Task<List<Menu>> AuthMenuAsync(int groupId)
        {
            IEnumerable<Menu> result = await _db.GetData<Menu, dynamic>("usp_Select_UserAuthorization", new { groupid = groupId });
            List<Menu> resultMenu = result.ToList();
            return resultMenu;
        }
    }
}
