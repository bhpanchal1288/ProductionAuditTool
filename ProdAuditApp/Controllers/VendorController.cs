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

        [HttpPost]
        public async Task<IActionResult> Save(Vendor vendor)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            if (vendor.vendorId == 0)
            {
                vendor.createdBy = Convert.ToInt32(userID);
                await _vendorRepository.InsertAsync(vendor);
                TempData["SuccessMessage"] = "Vendor created successfully.";
            }
            else
            {
                vendor.updatedBy = Convert.ToInt32(userID);
                await _vendorRepository.UpdateAsync(vendor);
                TempData["SuccessMessage"] = "Vendor updated successfully.";
            }
            //return Ok();
            return RedirectToAction("Index", "Vendor", new { id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Vendor vendor)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            vendor.updatedBy = Convert.ToInt32(userID);
            await _vendorRepository.DeleteAsync(vendor);
            TempData["SuccessMessage"] = "Vendor deleted successfully.";
            return RedirectToAction("Index", "Vendor", new { id = 0 });
        }
    }
}
