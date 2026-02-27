using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.SubGroupRepository;
using ProdAuditApp.Data.Repository.UserRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            var groupData = await _userRepository.GetUserGroupDropdownItemsAsync();
            ViewBag.GroupList = new SelectList(groupData, "valueId", "valueDec");

            if (Id != 0)
            {
                var data = await _userRepository.GetByIdAsync(Id);
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

        //[HttpPost]
        //public async Task<IActionResult> Save(User user)
        //{
        //    try
        //    {
        //        var userID = Convert.ToInt32(TempData["UserId"]);
        //        if (user.userid == 0)
        //        {
        //            user.createdby = Convert.ToInt32(userID);
        //            await _userRepository.InsertAsync(user);
        //            TempData["SuccessMessage"] = "User created successfully.";
        //        }
        //        else
        //        {
        //            user.updatedby = Convert.ToInt32(userID);
        //            await _userRepository.UpdateAsync(user);
        //            TempData["SuccessMessage"] = "User updated successfully.";
        //        }
        //        //return Ok();
        //        return RedirectToAction("Index", "User", new { id = 0 });
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(User user)
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

                if (user.userid == 0 || user.userid == null)
                {
                    //Insert Validation
                    user.createdby = userId;

                    response = await _userRepository.InsertAsync(user);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    user.updatedby = userId;

                    response = await _userRepository.UpdateAsync(user);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "User");
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
        //public async Task<IActionResult> Delete(User user)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    user.updatedby = Convert.ToInt32(userID);
        //    await _userRepository.DeleteAsync(user);
        //    TempData["SuccessMessage"] = "User deleted successfully.";
        //    return RedirectToAction("Index", "User", new { id = 0 });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(User user)
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
                user.updatedby = Convert.ToInt32(userId);
                response = await _userRepository.DeleteAsync(user);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "User", new { id = 0 });
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
