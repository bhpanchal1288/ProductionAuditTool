using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.Data.Repository.ProjectsRepository
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly ISqlDataAccess _db;

        public ProjectsRepository(ISqlDataAccess db)
        {
            _db = db;
        }

        public async Task<Projects> GetByIdAsync(int projectId)
        {
            var result = await _db.GetData<Projects, dynamic>("usp_Select_Project", new { projectid = projectId });
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<DropdownConfig>> GetClientDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "CLIENT",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<IEnumerable<DropdownConfig>> GetProductionHouseDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "PRODUCTIONHOUSE",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<IEnumerable<DropdownConfig>> GetProjectTypeDropdownItemsAsync()
        {
            var result = await _db.GetData<DropdownConfig, dynamic>("usp_Bind_DropDown"
                , new
                {
                    cmbName = "PROJECTTYPE",
                    FormName = "BLANK",
                    expr1 = (string)null,
                    expr2 = (string)null,
                    expr3 = (string)null,
                    expr4 = (string)null,
                    expr5 = (string)null
                });
            return result.ToList();
        }

        public async Task<ITMessage> InsertAsync(Projects project)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Project",
                            new
                            {
                                project.projectId
                                , project.projectCode
                                , project.projectName
                                , project.clientId
                                , project.phId
                                , project.projectType
                                , project.startDate
                                , project.tentendDate
                                , project.endDate
                                , project.createdBy
                                , project.updatedBy
                                , RequestType = "Insert"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Insert failed"
            };
        }

        public async Task<ITMessage> UpdateAsync(Projects project)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Project",
                            new
                            {
                                project.projectId
                                , project.projectCode
                                , project.projectName
                                , project.clientId
                                , project.phId
                                , project.projectType
                                , project.startDate
                                , project.tentendDate
                                , project.endDate
                                , project.createdBy
                                , project.updatedBy
                                , RequestType = "Update"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Update failed"
            };
        }

        public async Task<ITMessage> DeleteAsync(Projects project)
        {
            var res = await _db.SaveData<ITMessage>("usp_DML_Project",
                            new
                            {
                                project.projectId
                                , project.projectCode
                                , project.projectName
                                , project.clientId
                                , project.phId
                                , project.projectType
                                , project.startDate
                                , project.tentendDate
                                , project.endDate
                                , project.createdBy
                                , project.updatedBy
                                , RequestType = "Delete"
                            });
            return res ?? new ITMessage
            {
                i_IDENTITY = 0,
                msg = "Delete failed"
            };
        }
    }
}
