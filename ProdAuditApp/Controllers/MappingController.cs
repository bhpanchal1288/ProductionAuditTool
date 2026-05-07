using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ClientRepository;
using ProdAuditApp.Data.Repository.MappingRepository;
using ProdAuditApp.Data.Repository.SubGroupRepository;
using System.Collections.Generic;
using System.Linq;

namespace ProdAuditApp.UI.Controllers
{
    public class MappingController : Controller
    {
        private readonly IMappingRepository _mappingRepository;
        private readonly ISubGroupRepository _subGroupRepository;

        public MappingController(
            IMappingRepository mappingRepository,
            ISubGroupRepository subGroupRepository)
        {
            _mappingRepository = mappingRepository;
            _subGroupRepository = subGroupRepository;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            int projectId = Id ?? 0;

            // Dropdown data for the entry form
            var groupData = await _subGroupRepository.GetGroupDropdownItemsAsync();
            ViewBag.GroupList = new SelectList(groupData, "valueId", "valueDec");

            var categoryData = await _subGroupRepository.GetCategoryDropdownItemsAsync();
            // Mapping view uses this exact ViewBag key
            ViewBag.SubGroupCategoryList = new SelectList(categoryData, "valueId", "valueDec");

            // Initial state for subgroup dropdown; it will be refreshed by client-side JS.
            ViewBag.SubGroupList = new SelectList(Enumerable.Empty<DropdownConfig>(), "valueId", "valueDec");

            // Table data for the grid
            List<Mapping> mappingList = new();
            if (projectId != 0)
            {
                mappingList = await _mappingRepository.GetByIdAsync(projectId);
            }

            ViewBag.MappingList = mappingList;
            ViewBag.ProjectName = mappingList.FirstOrDefault()?.projectname;

            // Form model (blank). Grid is rendered from ViewBag.MappingList.
            return View(new Mapping { projectid = projectId });
        }

        [HttpGet]
        public async Task<IActionResult> GetSubGroupDropdownData(int groupid, int categoryid)
        {
            if (groupid == 0 || categoryid == 0)
            {
                return Json(Enumerable.Empty<DropdownConfig>());
            }

            var items = await _subGroupRepository.GetSubGroupDropdownItemsAsync(groupid, categoryid);
            return Json(items);
        }

        [HttpPost]
        public async Task<IActionResult> Save(Mapping mapping)
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
            if (mapping == null || mapping.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (mapping.mappingid == null || mapping.mappingid == 0)
            {
                mapping.createdBy = userId;
                // Insert
                response = await _mappingRepository.InsertAsync(mapping);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Insert failed";
                    //return await Index(mapping.projectid);
                    return View("Index");
                }
            }
            else
            {
                mapping.updatedBy = userId;
                // Update
                response = await _mappingRepository.UpdateAsync(mapping);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Update failed";
                    return View("Index");
                    //return await Index(mapping.projectid);
                }
            }

            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", "Mapping", new { Id = mapping.projectid });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Mapping mapping)
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
            if (mapping == null || mapping.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (mapping == null || mapping.projectid == 0 || mapping.mappingid == null || mapping.mappingid == 0)
            {
                return BadRequest("Mapping id missing.");
            }
            
            mapping.updatedBy = userId;
            response = await _mappingRepository.DeleteAsync(mapping);
            if (response == null || response.i_IDENTITY <= 0)
            {
                TempData["SuccessMessage"] = response?.msg ?? "Delete failed";
                return await Index(mapping.projectid);
            }

            TempData["SuccessMessage"] = response.msg;
            return RedirectToAction("Index", "Mapping", new { Id = mapping.projectid });
        }
    }
}
