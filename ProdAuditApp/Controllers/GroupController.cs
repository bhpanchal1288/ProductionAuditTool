using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.GroupRepository;
using ProdAuditApp.UI.Middleware;

namespace ProdAuditApp.UI.Controllers
{
    public class GroupController : Controller
    {
        private readonly IGroupRepository _groupRepository;
        public GroupController(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return View(); // New record screen
                }

                var data = await _groupRepository.GetByIdAsync(Id);

                if (data == null)
                {
                    return NotFound($"Record with Id {Id} not found.");
                }

                return View(data);                
            }
            catch (SqlException ex)
            {
                // Log DB error
                return StatusCode(500, "Database error occurred.");
            }
            catch (Exception ex)
            {
                // Log general error
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Save(Group group)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    if (group.groupId == 0)
        //    {
        //        group.createdBy = Convert.ToInt32(userID);
        //        var response = await _groupRepository.InsertAsync(group);
        //        TempData["SuccessMessage"] = response.msg;
        //    }
        //    else
        //    {
        //        group.updatedBy = Convert.ToInt32(userID);
        //        var response = await _groupRepository.UpdateAsync(group);
        //        TempData["SuccessMessage"] = response.msg;
        //    }
        //    //return Ok();
        //    return RedirectToAction("Index", "Group", new { id = 0 });

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Group group)
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

                if (group.groupId == 0 || group.groupId == null)
                {
                    //Insert Validation
                    group.createdBy = userId;

                    response = await _groupRepository.InsertAsync(group);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    group.updatedBy = userId;

                    response = await _groupRepository.UpdateAsync(group);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Group");
            }
            catch (Exception ex)
            {
                // 5️⃣ Exception Handling
                ModelState.AddModelError("", "An unexpected error occurred.");
                // Log error here (Serilog / NLog recommended)
                return View("Index");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Delete(Group group)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    group.updatedBy = Convert.ToInt32(userID);
        //    var response = await _groupRepository.DeleteAsync(group);
        //    TempData["SuccessMessage"] = response.msg;
        //    return RedirectToAction("Index", "Group", new { id = 0 });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(Group group)
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
                group.updatedBy = Convert.ToInt32(userId);
                response = await _groupRepository.DeleteAsync(group);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Group", new { id = 0 });
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
