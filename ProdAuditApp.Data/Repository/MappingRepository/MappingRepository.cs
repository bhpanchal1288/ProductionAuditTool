using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.MappingRepository
{
    public class MappingRepository : IMappingRepository
    {
        private readonly ISqlDataAccess _db;
        public MappingRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<List<Mapping>> GetByIdAsync(int projectId)
        {
            var result = await _db.GetData<Mapping, dynamic>("usp_Select_Mapping", new { projectId = projectId });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(Mapping mapping)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Mapping",
                new
                {
                    mapping.mappingid,
                    mapping.projectid,
                    mapping.groupid,
                    mapping.subgroupcategoryid,
                    mapping.subgroupid,
                    mapping.particulars,
                    mapping.createdBy,
                    mapping.updatedBy,
                    RequestType = "Insert"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Insert failed" };
        }

        public async Task<ITMessage> UpdateAsync(Mapping mapping)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Mapping",
                new
                {
                    mapping.mappingid,
                    mapping.projectid,
                    mapping.groupid,
                    mapping.subgroupcategoryid,
                    mapping.subgroupid,
                    mapping.particulars,
                    mapping.createdBy,
                    mapping.updatedBy,
                    RequestType = "Update"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Update failed" };
        }

        public async Task<ITMessage> DeleteAsync(Mapping mapping)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Mapping",
                new
                {
                    mapping.mappingid,
                    mapping.projectid,
                    mapping.groupid,
                    mapping.subgroupcategoryid,
                    mapping.subgroupid,
                    mapping.particulars,
                    mapping.createdBy,
                    mapping.updatedBy,
                    RequestType = "Delete"
                });

            return res ?? new ITMessage { i_IDENTITY = 0, msg = "Delete failed" };
        }

        
    }
}
