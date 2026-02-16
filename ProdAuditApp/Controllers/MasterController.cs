using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.MasterConfigRepository;
using System.Text.Json;

namespace ProdAuditApp.UI.Controllers
{
    public class MasterController : Controller
    {
        private readonly IMasterConfigService _service;
        private readonly ILogger<MasterController> _logger;

        public MasterController(IMasterConfigService service, ILogger<MasterController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string masterCode = null, int page = 1, int pageSize = 10, string sort = "", string dir = "asc")
        {
            ViewData["MasterCode"] = masterCode;
            ViewData["Title"] = "Master Data";
            ViewData["CurrentPage"] = page;
            ViewData["PageSize"] = pageSize;
            ViewData["Sort"] = sort;
            ViewData["Dir"] = dir;

            // Load data server-side if masterCode is provided
            if (!string.IsNullOrEmpty(masterCode))
            {
                try
                {
                    var result = await _service.GetDataAsync(masterCode, page, pageSize, sort, dir);
                    
                    // Store the result in ViewData for server-side rendering
                    if (result != null)
                    {
                        dynamic resultDynamic = result;
                        
                        // Convert dynamic data to serializable format
                        var columns = (IEnumerable<MasterColumnDefinition>)resultDynamic.Columns;
                        var data = (IEnumerable<dynamic>)resultDynamic.Data;
                        var totalRecords = (int)resultDynamic.TotalRecords;

                        // Convert dynamic rows to dictionaries for JSON serialization
                        var dataList = new List<Dictionary<string, object>>();
                        foreach (var row in data)
                        {
                            var dict = new Dictionary<string, object>();
                            var rowDict = (IDictionary<string, object>)row;
                            foreach (var kvp in rowDict)
                            {
                                dict[kvp.Key] = kvp.Value ?? DBNull.Value;
                            }
                            dataList.Add(dict);
                        }

                        var jsonResult = new
                        {
                            columns = columns.Select(c => new
                            {
                                columnId = c.ColumnId,
                                masterId = c.MasterId,
                                columnName = c.ColumnName,
                                displayName = c.DisplayName,
                                dataType = c.DataType,
                                isVisible = c.IsVisible,
                                isSortable = c.IsSortable,
                                isFilterable = c.IsFilterable,
                                displayOrder = c.DisplayOrder
                            }),
                            data = dataList,
                            totalRecords = totalRecords
                        };

                        ViewData["MasterData"] = result;
                        
                        var jsonOptions = new JsonSerializerOptions
                        {
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                            WriteIndented = false
                        };
                        ViewData["MasterDataJson"] = JsonSerializer.Serialize(jsonResult, jsonOptions);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error loading master data for code: {MasterCode}", masterCode);
                    ViewData["MasterDataError"] = "An error occurred while loading data: " + ex.Message;
                }
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetMasterData(
        string masterCode,
        int page = 1,
        int pageSize = 10,
        string sort = "",
        string dir = "asc")
        {
            if (string.IsNullOrEmpty(masterCode))
            {
                return BadRequest(new { error = "MasterCode is required" });
            }

            var result = await _service.GetDataAsync(
                masterCode, page, pageSize, sort, dir);

            return Ok(result);
        }
    }
}
