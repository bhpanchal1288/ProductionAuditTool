using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.LocationBudgetRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class LocationBudgetController : Controller
    {
        private readonly ILocationBudgetRepository _locationBudgetRepository;

        public LocationBudgetController(ILocationBudgetRepository locationBudgetRepository)
        {
            _locationBudgetRepository = locationBudgetRepository;
        }
        public async Task<ActionResult> Index(int? Id)
        {
            int projectId = Id ?? 0;

            // Dropdown data for the entry form
            var locationTypeData = await _locationBudgetRepository.GetLocationTypeDropdownItemsAsync();
            ViewBag.LocationTypeList = new SelectList(locationTypeData, "valueId", "valueDec");
                        
            // Table data for the grid
            List<LocationBudget> locationBudgetList = new();
            if (projectId != 0)
            {
                locationBudgetList = await _locationBudgetRepository.GetByIdAsync(projectId);
            }

            ViewBag.LocationBudgetList = locationBudgetList;
            ViewBag.ProjectName = locationBudgetList.FirstOrDefault()?.projectname;

            // Form model (blank). Grid is rendered from ViewBag.MappingList.
            return View(new LocationBudget { projectid = projectId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(LocationBudget locationBudget)
        {
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

            // Ensure we have a project context for refresh/redirects.
            if (locationBudget == null || locationBudget.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (locationBudget.locationbudgetid == null || locationBudget.locationbudgetid == 0)
            {
                locationBudget.createdBy = userId;
                // Insert
                response = await _locationBudgetRepository.InsertAsync(locationBudget);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Insert failed";
                    //return await Index(mapping.projectid);
                    return View("Index");
                }
            }
            else
            {
                locationBudget.updatedBy = userId;
                // Update
                response = await _locationBudgetRepository.UpdateAsync(locationBudget);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Update failed";
                    return View("Index");
                    //return await Index(mapping.projectid);
                }
            }

            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", "LocationBudget", new { Id = locationBudget.projectid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(LocationBudget locationBudget)
        {
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

            // Ensure we have a project context for refresh/redirects.
            if (locationBudget == null || locationBudget.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (locationBudget == null || locationBudget.projectid == 0 || locationBudget.locationbudgetid == 0)
            {
                return BadRequest("location budget is missing.");
            }

            locationBudget.updatedBy = userId;
            response = await _locationBudgetRepository.DeleteAsync(locationBudget);
            if (response == null || response.i_IDENTITY <= 0)
            {
                TempData["SuccessMessage"] = response?.msg ?? "Delete failed";
                return await Index(locationBudget.projectid);
            }

            TempData["SuccessMessage"] = response.msg;
            return RedirectToAction("Index", "LocationBudget", new { Id = locationBudget.projectid });
        }
    }
}
