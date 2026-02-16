using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.GroupRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if (Id != 0)
            {
                var data = await _groupRepository.GetByIdAsync(Id);
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
        public async Task<IActionResult> Save(Group group)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            if (group.groupId == 0)
            {
                group.createdBy = Convert.ToInt32(userID);
                await _groupRepository.InsertAsync(group);
                TempData["SuccessMessage"] = "Group created successfully.";
            }
            else
            {
                group.updatedBy = Convert.ToInt32(userID);
                await _groupRepository.UpdateAsync(group);
                TempData["SuccessMessage"] = "Group updated successfully.";
            }
            //return Ok();
            return RedirectToAction("Index", "Group", new { id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Group group)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            group.updatedBy = Convert.ToInt32(userID);
            await _groupRepository.DeleteAsync(group);
            TempData["SuccessMessage"] = "Group deleted successfully.";
            return RedirectToAction("Index", "Group", new { id = 0 });
        }
    }
}
