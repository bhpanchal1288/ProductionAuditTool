using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ProductionHouseRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class ProductionHouseController : Controller
    {
        private readonly IProductionHouseRepository _productionHouseRepository;

        public ProductionHouseController(IProductionHouseRepository productionHouseRepository)
        {
            _productionHouseRepository = productionHouseRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if (Id != 0)
            {
                var data = await _productionHouseRepository.GetByIdAsync(Id);
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
        //public async Task<IActionResult> Save(ProductionHouse productionHouse)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);//User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (productionHouse.phId == 0)
        //    {
        //        productionHouse.createdBy = Convert.ToInt32(userID);
        //        await _productionHouseRepository.InsertAsync(productionHouse);
        //        TempData["SuccessMessage"] = "ProductionHouse created successfully.";
        //    }
        //    else
        //    {
        //        productionHouse.updatedBy = Convert.ToInt32(userID);
        //        await _productionHouseRepository.UpdateAsync(productionHouse);
        //        TempData["SuccessMessage"] = "ProductionHouse updated successfully.";
        //    }
        //    //return Ok();
        //    return RedirectToAction("Index", "ProductionHouse", new { id = 0 });

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(ProductionHouse productionHouse)
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

                if (productionHouse.phId== 0 || productionHouse.phId == null)
                {
                    //Insert Validation
                    productionHouse.createdBy = userId;

                    response = await _productionHouseRepository.InsertAsync(productionHouse);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    productionHouse.updatedBy = userId;

                    response = await _productionHouseRepository.UpdateAsync(productionHouse);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "ProductionHouse");
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
        //public async Task<IActionResult> Delete(ProductionHouse productionHouse)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    productionHouse.updatedBy = Convert.ToInt32(userID);
        //    await _productionHouseRepository.DeleteAsync(productionHouse);
        //    TempData["SuccessMessage"] = "ProductionHouse deleted successfully.";
        //    return RedirectToAction("Index", "ProductionHouse", new { id = 0 });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(ProductionHouse productionHouse)
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
                productionHouse.updatedBy = Convert.ToInt32(userId);
                response = await _productionHouseRepository.DeleteAsync(productionHouse);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "ProductionHouse", new { id = 0 });
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
