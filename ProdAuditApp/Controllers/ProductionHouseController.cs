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

        [HttpPost]
        public async Task<IActionResult> Save(ProductionHouse productionHouse)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);//User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (productionHouse.phId == 0)
            {
                productionHouse.createdBy = Convert.ToInt32(userID);
                await _productionHouseRepository.InsertAsync(productionHouse);
                TempData["SuccessMessage"] = "ProductionHouse created successfully.";
            }
            else
            {
                productionHouse.updatedBy = Convert.ToInt32(userID);
                await _productionHouseRepository.UpdateAsync(productionHouse);
                TempData["SuccessMessage"] = "ProductionHouse updated successfully.";
            }
            //return Ok();
            return RedirectToAction("Index", "ProductionHouse", new { id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductionHouse productionHouse)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            productionHouse.updatedBy = Convert.ToInt32(userID);
            await _productionHouseRepository.DeleteAsync(productionHouse);
            TempData["SuccessMessage"] = "ProductionHouse deleted successfully.";
            return RedirectToAction("Index", "ProductionHouse", new { id = 0 });
        }
    }
}
