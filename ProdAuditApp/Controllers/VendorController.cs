using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ClientRepository;
using ProdAuditApp.Data.Repository.VendorRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class VendorController : Controller
    {
        private readonly IVendorRepository _vendorRepository;

        public VendorController(IVendorRepository vendorRepository)
        {
            _vendorRepository = vendorRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if (Id != 0)
            {
                var data = await _vendorRepository.GetByIdAsync(Id);
                if (data == null)
                {
                    return NotFound();
                }
                return View(data);
            }
            else
            {
                return View();
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Save(Vendor vendor)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    if (vendor.vendorId == 0)
        //    {
        //        vendor.createdBy = Convert.ToInt32(userID);
        //        await _vendorRepository.InsertAsync(vendor);
        //        TempData["SuccessMessage"] = "Vendor created successfully.";
        //    }
        //    else
        //    {
        //        vendor.updatedBy = Convert.ToInt32(userID);
        //        await _vendorRepository.UpdateAsync(vendor);
        //        TempData["SuccessMessage"] = "Vendor updated successfully.";
        //    }
        //    //return Ok();
        //    return RedirectToAction("Index", "Vendor", new { id = 0 });

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Vendor vendor)
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

                if (vendor.vendorId == 0 || vendor.vendorId == null)
                {
                    //Insert Validation
                    vendor.createdBy = userId;

                    response = await _vendorRepository.InsertAsync(vendor);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    vendor.updatedBy = userId;

                    response = await _vendorRepository.UpdateAsync(vendor);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Vendor");
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
        //public async Task<IActionResult> Delete(Vendor vendor)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    vendor.updatedBy = Convert.ToInt32(userID);
        //    await _vendorRepository.DeleteAsync(vendor);
        //    TempData["SuccessMessage"] = "Vendor deleted successfully.";
        //    return RedirectToAction("Index", "Vendor", new { id = 0 });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(Vendor vendor)
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
                vendor.updatedBy = Convert.ToInt32(userId);
                response = await _vendorRepository.DeleteAsync(vendor);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Vendor", new { id = 0 });
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
