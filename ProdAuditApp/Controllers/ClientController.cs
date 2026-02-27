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

        //[HttpPost]
        //public async Task<IActionResult> Save(Client client)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);//User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    if (client.clientId == 0)
        //    {
        //        client.createdBy = Convert.ToInt32(userID);
        //        await _clientRepository.InsertAsync(client);
        //        TempData["SuccessMessage"] = "Client created successfully.";
        //    }
        //    else
        //    {
        //        client.updatedBy = Convert.ToInt32(userID);
        //        await _clientRepository.UpdateAsync(client);
        //        TempData["SuccessMessage"] = "Client updated successfully.";
        //    }
        //    //return Ok();
        //    return RedirectToAction("Index", "Client", new { id = 0 });

        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(Client client)
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

                if (client.clientId == 0 || client.clientId == null)
                {
                    //Insert Validation
                    client.createdBy = userId;

                    response = await _clientRepository.InsertAsync(client);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Insert failed.");
                        return View("Index");
                    }
                }
                else
                {
                    // 4️⃣ Update Validation
                    client.updatedBy = userId;

                    response = await _clientRepository.UpdateAsync(client);

                    if (response == null || response.i_IDENTITY <= 0)
                    {
                        ModelState.AddModelError("", response?.msg ?? "Update failed.");
                        return View("Index");
                    }
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Client");
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
        //public async Task<IActionResult> Delete(Client client)
        //{
        //    var userID = Convert.ToInt32(TempData["UserId"]);
        //    client.updatedBy = Convert.ToInt32(userID);
        //    await _clientRepository.DeleteAsync(client);
        //    TempData["SuccessMessage"] = "Client deleted successfully.";
        //    return RedirectToAction("Index", "Client", new { id = 0 });
        //}

        [HttpPost]
        public async Task<IActionResult> Delete(Client client)
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
                client.updatedBy = Convert.ToInt32(userId);
                response = await _clientRepository.DeleteAsync(client);

                if (response == null || response.i_IDENTITY <= 0)
                {
                    ModelState.AddModelError("", response?.msg ?? "Delete failed.");
                    return View("Index");
                }

                TempData["SuccessMessage"] = response.msg;
                return RedirectToAction("Index", "Client", new { id = 0 });
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
