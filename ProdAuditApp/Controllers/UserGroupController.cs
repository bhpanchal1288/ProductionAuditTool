using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.UserGroupRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class UserGroupController : Controller
    {
        private readonly IUserGroupRepository _userGroupRepository;
        public UserGroupController(IUserGroupRepository userGroupRepository)
        {
            _userGroupRepository = userGroupRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            try
            {
                if (Id <= 0)
                {
                    return View(); // New record screen
                }

                var data = await _userGroupRepository.GetByIdAsync(Id);

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
        //public async Task<IActionResult> Save(UserGroup userGroup)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return View(userGroup);
        //    }

        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    if (userGroup.groupId == 0)
        //    {
        //        userGroup.createdBy = Convert.ToInt32(userID);
        //        var response =  await _userGroupRepository.InsertAsync(userGroup);
        //        TempData["SuccessMessage"] = response.msg;
        //    }
        //    else
        //    {
        //        userGroup.updatedBy = Convert.ToInt32(userID);
        //        var response = await _userGroupRepository.UpdateAsync(userGroup);
        //        TempData["SuccessMessage"] = response.msg;
        //    }

        //    //return Ok();
        //    return RedirectToAction("Index", "UserGroup", new { id = 0 });

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UserGroup userGroup)
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

                if (userGroup.groupId == 0 || userGroup.groupId == null)
                {
                    //Insert Validation
                    userGroup.createdBy = userId;

                    response = await _userGroupRepository.InsertAsync(userGroup);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    userGroup.updatedBy = userId;

                    response = await _userGroupRepository.UpdateAsync(userGroup);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "UserGroup");
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
        public async Task<IActionResult> Delete(UserGroup userGroup)
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
                userGroup.updatedBy = Convert.ToInt32(userId);
                response = await _userGroupRepository.DeleteAsync(userGroup);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "UserGroup", new { id = 0 });
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
