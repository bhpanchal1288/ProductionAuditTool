using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ContractsRepository;
using ProdAuditApp.Data.Repository.DropdownRepository;
using ProdAuditApp.Data.Repository.SubGroupRepository;
using ProdAuditApp.Data.Repository.VendorRepository;
using System.Diagnostics.Contracts;
using System.Linq;

namespace ProdAuditApp.UI.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IContractsRepository _contractsRepository;
        private readonly ISubGroupRepository _subGroupRepository;
        private readonly IDropdownRepository _dropdownRepository;

        public ContractsController(IContractsRepository contractsRepository, ISubGroupRepository subGroupRepository, IDropdownRepository dropdownRepository)
        {
            _contractsRepository = contractsRepository;
            _subGroupRepository = subGroupRepository;
            _dropdownRepository = dropdownRepository;
        }

        public async Task<IActionResult> Index(int? Id)
        {
            int contractId = Id ?? 0;

            // Populate dropdowns
            var groupData = await _subGroupRepository.GetGroupDropdownItemsAsync();
            ViewBag.GroupList = new SelectList(groupData, "valueId", "valueDec");

            var categoryData = await _subGroupRepository.GetCategoryDropdownItemsAsync();
            ViewBag.CategoryList = new SelectList(categoryData, "valueId", "valueDec");

            var subGroupData = await _subGroupRepository.GetSubGroupDropdownItemsAsync(0, 0);
            ViewBag.SubGroupList = subGroupData.Select(s => new { Value = s.valueId, Text = s.valueDec }).ToList();

            var uomData = await _dropdownRepository.GetDropdownDataAsync("UOM", "BLANK");
            ViewBag.UOMList = new SelectList(uomData, "valueId", "valueDec");

            var projectData = await _dropdownRepository.GetDropdownDataAsync("PROJECTNAME", "BLANK");
            ViewBag.ProjectList = new SelectList(projectData, "valueId", "valueDec");

            var vendorData = await _dropdownRepository.GetDropdownDataAsync("VENDORNAME", "BLANK");
            ViewBag.VendorList = new SelectList(vendorData, "valueId", "valueDec");

            

            if (contractId != 0)
            {
                var contracts = await _contractsRepository.GetByContractAsync(contractId);
                ViewBag.ContractsList = contracts;
                if (contracts != null && contracts.Count > 0)
                {
                    var model = contracts.First();
                    var details = await _contractsRepository.GetDetailsByContractIdAsync(model.contractid ?? 0);
                    model.Details = details;
                    return View(model);
                }
            }

            //return View();
            return View(new Contracts { contractid = contractId });
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Contracts contract)
        {
            if (!ModelState.IsValid)
                return View("Index", contract);

            ITMessage response;
            int contractId = 0;
            
            if (contract.contractid == null || contract.contractid == 0)
            {
                contract.createdby = Convert.ToInt32(TempData["userid"]);
                response = await _contractsRepository.InsertAsync(contract);

                contractId = response.i_IDENTITY;
                

                // Save details
                if (contract.Details != null && contract.Details.Count > 0)
                {
                    foreach (var d in contract.Details)
                    {
                        d.contractid = contractId;
                        d.createdby = Convert.ToInt32(TempData["userid"]);
                        await ((ContractsRepository)_contractsRepository).SaveDetailAsync(d, "Insert");
                    }
                }

            }
            else
            {
                contract.updatedby = Convert.ToInt32(TempData["userid"]);
                response = await _contractsRepository.UpdateAsync(contract);

                contractId = response.i_IDENTITY;

                // Save details
                if (contract.Details != null && contract.Details.Count > 0)
                {
                    foreach (var d in contract.Details)
                    {
                        d.contractid = contractId;
                        d.updatedby = Convert.ToInt32(TempData["userid"]);
                        await ((ContractsRepository)_contractsRepository).SaveDetailAsync(d, "Update");
                    }
                }
            }

            if (response == null || response.i_IDENTITY <= 0)
            {                
                TempData["SuccessMessage"] = response?.msg ?? "Save failed";
                return RedirectToAction("Index", new { Id = contract.projectid });
            }
            

            TempData["SuccessMessage"] = "Saved successfully.";
            return RedirectToAction("Index", new { Id = contract.projectid });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Contracts contract)
        {
            if (contract == null || contract.contractid == null)
                return BadRequest();

            contract.updatedby = Convert.ToInt32(TempData["userid"]);
            var response = await _contractsRepository.DeleteAsync(contract);
            TempData["SuccessMessage"] = response?.msg ?? "Deleted";
            return RedirectToAction("Index", new { Id = contract.projectid });
        }
    }
}
