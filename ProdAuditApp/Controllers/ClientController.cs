using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.Data.Model.Domain;
using ProdAuditApp.Data.Repository.ClientRepository;

namespace ProdAuditApp.UI.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientRepository _clientRepository;

        public ClientController(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }
        public async Task<IActionResult> Index(int Id)
        {
            if (Id != 0)
            {
                var data = await _clientRepository.GetByIdAsync(Id);
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
        public async Task<IActionResult> Save(Client client)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);//User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (client.clientId == 0)
            {
                client.createdBy = Convert.ToInt32(userID);
                await _clientRepository.InsertAsync(client);
                TempData["SuccessMessage"] = "Client created successfully.";
            }
            else
            {
                client.updatedBy = Convert.ToInt32(userID);
                await _clientRepository.UpdateAsync(client);
                TempData["SuccessMessage"] = "Client updated successfully.";
            }
            //return Ok();
            return RedirectToAction("Index", "Client", new { id = 0 });

        }

        [HttpPost]
        public async Task<IActionResult> Delete(Client client)
        {
            var userID = Convert.ToInt32(TempData["UserId"]);
            client.updatedBy = Convert.ToInt32(userID);
            await _clientRepository.DeleteAsync(client);
            TempData["SuccessMessage"] = "Client deleted successfully.";
            return RedirectToAction("Index", "Client", new { id = 0 });
        }
    }
}
