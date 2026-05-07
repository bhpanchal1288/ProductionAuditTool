using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.BudgetRepository;
using ProdAuditApp.Data.Repository.CallSheetRepository;
using ProdAuditApp.Data.Repository.DropdownRepository;
using System.Collections.Generic;
using System.Linq;

namespace ProdAuditApp.UI.Controllers
{
    public class CallSheetController : Controller
    {
        private readonly ICallSheetRepository _callSheetRepository;
        private readonly IDropdownRepository _dropdownRepository;

        public CallSheetController(ICallSheetRepository callSheetRepository, IDropdownRepository dropdownRepository)
        {
            _callSheetRepository = callSheetRepository;
            _dropdownRepository = dropdownRepository;
        }

        [HttpGet] 
        public async Task<IActionResult> Index(int Id)
        {
            //int callsheetId = Id ?? 0;

            // Dropdown data for the entry form
            var projectData = await _dropdownRepository.GetDropdownDataAsync("PROJECTNAME", "BLANK");
            ViewBag.ProjectList = new SelectList(projectData, "valueId", "valueDec");

            ViewBag.LocationList = new SelectList(Enumerable.Empty<DropdownConfig>(), "valueId", "valueDec");

            // Mapping view uses this exact ViewBag key
            var shiftData = await _dropdownRepository.GetDropdownDataAsync("SHIFTTYPE", "BLANK");
            ViewBag.ShiftList = new SelectList(shiftData, "valueId", "valueDec");


            // Table data for the grid
            //List<CallSheet> callsheetList = new();
            if (Id != 0)
            {
                var data = await _callSheetRepository.GetCallSheetByIdAsync(Id);
                if (data == null)
                {
                    return NotFound();
                }
                // load locations for the callsheet's project so the Location dropdown can be populated
                if (data.projectid.HasValue && data.projectid.Value > 0)
                {
                    var locationData = await _dropdownRepository.GetDropdownDataAsync("LOCATION", "BLANK", data.projectid.Value.ToString());
                    ViewBag.LocationList = new SelectList(locationData, "valueId", "valueDec");
                }
                else
                {
                    ViewBag.LocationList = new SelectList(Enumerable.Empty<SelectListItem>());
                }

                // pass selected location id to the view for pre-selection in case values are strings
                ViewBag.SelectedLocationId = data.locationid?.ToString() ?? string.Empty;

                return View(data);
            }
            else
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetLocationDropdownData(int projectid)
        {
            if (projectid == 0)
            {
                return Json(Enumerable.Empty<DropdownConfig>());
            }

            var items = await _dropdownRepository.GetDropdownDataAsync("LOCATION", "BLANK", projectid.ToString());
            return Json(items);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CallSheet callSheet)
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
            if (callSheet == null || callSheet.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (callSheet.callsheetid == null || callSheet.callsheetid == 0)
            {
                callSheet.createdBy = userId;
                // Insert
                response = await _callSheetRepository.InsertAsync(callSheet);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Insert failed";
                    //return await Index(mapping.projectid);
                    return View("Index");
                }
            }
            else
            {
                callSheet.updatedBy = userId;
                // Update
                response = await _callSheetRepository.UpdateAsync(callSheet);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Update failed";
                    return View("Index");
                    //return await Index(mapping.projectid);
                }
            }

            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", "CallSheet");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(CallSheet callSheet)
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
            if (callSheet == null || callSheet.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            callSheet.updatedBy = userId;
            response = await _callSheetRepository.DeleteAsync(callSheet);
            if (response == null || response.i_IDENTITY <= 0)
            {
                TempData["SuccessMessage"] = response?.msg ?? "Delete failed";
                return View("Index");
            }

            TempData["SuccessMessage"] = response.msg;
            return RedirectToAction("Index", "CallSheet", new { Id = 0 });
        }
    }
}
