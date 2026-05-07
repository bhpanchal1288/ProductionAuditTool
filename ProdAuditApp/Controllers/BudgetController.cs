using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.BudgetRepository;
using ProdAuditApp.Data.Repository.ClientRepository;
using ProdAuditApp.Data.Repository.MappingRepository;
using ProdAuditApp.Data.Repository.SubGroupRepository;
using System.Collections.Generic;
using System.Linq;

namespace ProdAuditApp.UI.Controllers
{
    public class BudgetController : Controller
    {
        private readonly IBudgetRepository _budgetRepository;
        private readonly ISubGroupRepository _subGroupRepository;

        public BudgetController(
            IBudgetRepository budgetRepository,
            ISubGroupRepository subGroupRepository)
        {
            _budgetRepository = budgetRepository;
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
            List<Budget> mappingList = new();
            if (projectId != 0)
            {
                mappingList = await _budgetRepository.GetByIdAsync(projectId);
            }

            ViewBag.MappingList = mappingList;
            ViewBag.ProjectName = mappingList.FirstOrDefault()?.projectname;

            // Form model (blank). Grid is rendered from ViewBag.MappingList.
            return View(new Budget { projectid = projectId });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromBody] Budget budget)
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
            if (budget == null || budget.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            //if (mapping.mappingid == null || mapping.mappingid == 0)
            //{
            //    mapping.createdBy = userId;
            //    // Insert
            //    response = await _mappingRepository.InsertAsync(mapping);
            //    if (response == null || response.i_IDENTITY <= 0)
            //    {
            //        TempData["SuccessMessage"] = response?.msg ?? "Insert failed";
            //        //return await Index(mapping.projectid);
            //        return View("Index");
            //    }
            //}
            //else
            //{
                budget.updatedBy = userId;
                // Update
                response = await _budgetRepository.UpdateAsync(budget);
                if (response == null || response.i_IDENTITY <= 0)
                {
                    TempData["SuccessMessage"] = response?.msg ?? "Update failed";
                    return View("Index");
                    //return await Index(mapping.projectid);
                }
            //}

            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", "Budget", new { Id = budget.projectid });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromBody] Budget budget)
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
            if (budget == null || budget.projectid == 0)
            {
                return BadRequest("Project context missing.");
            }

            ITMessage response;

            if (budget == null || budget.budgetid == 0 || budget.projectid == 0 || budget.mappingid == null || budget.mappingid == 0)
            {
                return BadRequest("Budget id missing.");
            }

            budget.updatedBy = userId;
            response = await _budgetRepository.DeleteAsync(budget);
            if (response == null || response.i_IDENTITY <= 0)
            {
                TempData["SuccessMessage"] = response?.msg ?? "Delete failed";
                return await Index(budget.projectid);
            }

            TempData["SuccessMessage"] = response.msg;
            return RedirectToAction("Index", "Mapping", new { Id = budget.projectid });
        }
    }
}
