using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.GroupRepository;
using ProdAuditApp.Data.Repository.SubGroupRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class SubGroupController : Controller
    {
        private readonly ISubGroupRepository _subGroupRepository;
        public SubGroupController(ISubGroupRepository subGroupRepository)
        {
            _subGroupRepository = subGroupRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if (Id != 0)
            {
                var data = await _subGroupRepository.GetByIdAsync(Id);
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

        public async Task<List<DropdownConfig>> GetDropdownDataAsync(string cmbName
            , string FormName
            , string expr1 
            , string expr2 
            , string expr3 
            , string expr4 
            , string expr5)
        {
            if (cmbName != "")
            {
                var data = await _subGroupRepository.GetByIdAsync(Id);
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
        public async Task<IActionResult> Save(SubGroup subGroup)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            if (subGroup.subgroupid == 0)
            {
                subGroup.createdby = Convert.ToInt32(userID);
                await _subGroupRepository.InsertAsync(subGroup);
                TempData["SuccessMessage"] = "Sub Group created successfully.";
            }
            else
            {
                subGroup.updatedby = Convert.ToInt32(userID);
                await _subGroupRepository.UpdateAsync(subGroup);
                TempData["SuccessMessage"] = "Sub Group updated successfully.";
            }
            //return Ok();
            return RedirectToAction("Index", "SubGroup", new { id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(SubGroup subGroup)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            subGroup.updatedby = Convert.ToInt32(userID);
            await _subGroupRepository.DeleteAsync(subGroup);
            TempData["SuccessMessage"] = "Sub Group deleted successfully.";
            return RedirectToAction("Index", "Group", new { id = 0 });
        }
    }
}
