using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ProjectsRepository;


namespace ProdAuditApp.UI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectsRepository _projectRepository;

        public ProjectsController(IProjectsRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id = null)
        {
            var clientData = await _projectRepository.GetClientDropdownItemsAsync();
            ViewBag.ClientList = new SelectList(clientData, "valueId", "valueDec");

            var productionHouseData = await _projectRepository.GetProductionHouseDropdownItemsAsync();
            ViewBag.ProductionHouseList = new SelectList(productionHouseData, "valueId", "valueDec");

            var projectTypeData = await _projectRepository.GetProjectTypeDropdownItemsAsync();
            ViewBag.ProjectTypeList = new SelectList(projectTypeData, "valueId", "valueDec");


            if (id.HasValue && id.Value != 0)
            {
                var data = await _projectRepository.GetByIdAsync(id.Value);
                if (data == null)
                {
                    return NotFound();
                }
                //return Json(data);
                return View(data);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Projects project)
        {
            //Model validation (Data Annotations)
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            //Check Session/TempData UserId
            if (TempData["userid"] == null)
            {
                ModelState.AddModelError("", "Session expired. Please login again.");
                return View("Index");
            }

            int userId;
            if (!int.TryParse(TempData["userid"].ToString(), out userId))
            {
                ModelState.AddModelError("", "Invalid user session.");
                return View("Index");
            }

            try
            {
                ITMessage response;

                if (project.projectId == 0 || project.projectId == null)
                {
                    //Insert Validation
                    project.createdBy = userId;

                    response = await _projectRepository.InsertAsync(project);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    project.updatedBy = userId;

                    response = await _projectRepository.UpdateAsync(project);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Projects");
            }
            catch (Exception ex)
            {
                // 5️⃣ Exception Handling
                ModelState.AddModelError("", "An unexpected error occurred.");
                // Log error here (Serilog / NLog recommended)
                return View("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Projects project)
        {

            //Model validation (Data Annotations)
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            //Check Session/TempData UserId
            if (TempData["UserId"] == null)
            {
                ModelState.AddModelError("", "Session expired. Please login again.");
                return View("Index");
            }

            int userId;
            if (!int.TryParse(TempData["userid"].ToString(), out userId))
            {
                ModelState.AddModelError("", "Invalid user session.");
                return View("Index");
            }

            try
            {
                ITMessage response;

                //var userID = Convert.ToInt32(TempData["UserId"]);
                project.updatedBy = Convert.ToInt32(userId);
                response = await _projectRepository.DeleteAsync(project);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Projects", new { id = 0 });
            }
            catch (Exception ex)
            {
                // 5️⃣ Exception Handling
                ModelState.AddModelError("", "An unexpected error occurred.");
                // Log error here (Serilog / NLog recommended)
                return View("Index");
            }
        }
    }
}
