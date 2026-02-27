namespace ProdAuditApp.Data.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<IEnumerable<T>> GetData<T, P>(string spName, P parameters, string connectionId = "DefaultConnection");

        Task Save<T>(string spName, T parameters, string connectionId = "DefaultConnection");
        Task<ITMessage> SaveData<ITMessage>(string spName, object parameters, string connectionId = "DefaultConnection");
    }
}
