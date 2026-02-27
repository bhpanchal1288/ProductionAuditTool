using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly ISqlDataAccess _db;

        public UserRepository(ISqlDataAccess db) 
        { 
            _db = db;
        }

        public async Task<User> GetByIdAsync(int userId)
        {
            var result = await _db.GetData<User, dynamic>("usp_Select_User", new { userid = userId });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<DropdownConfig>> GetUserGroupDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "USERGROUP",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(User user)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserMaster",
                            new
                            {
                                user.userid,
                                user.groupid,
                                user.firstname,
                                user.lastname,
                                user.mobile,
                                user.emailid,
                                user.password,
                                user.createdby,
                                user.updatedby,
                                RequestType = "Insert"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(User user)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserMaster",
                            new
                            {
                                user.userid,
                                user.groupid,
                                user.firstname,
                                user.lastname,
                                user.mobile,
                                user.emailid,
                                user.password,
                                user.createdby,
                                user.updatedby,
                                RequestType = "Update"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(User user)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_UserMaster",
                            new
                            {
                                user.userid,
                                user.groupid,
                                user.firstname,
                                user.lastname,
                                user.mobile,
                                user.emailid,
                                user.password,
                                user.createdby,
                                user.updatedby,
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
