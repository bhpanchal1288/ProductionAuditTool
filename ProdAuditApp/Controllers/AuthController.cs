using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.UI.Models;
using ProdAuditApp.Data.Repository.AuthRepository;
using ProdAuditApp.Data.Model.Domain;

namespace ProdAuditApp.UI.Controllers;

public class AuthController : Controller
{
  private readonly ILogger<AuthController> _logger;
  private readonly IAuthRepository _authRepository;  
  public AuthController(IAuthRepository authRepository, ILogger<AuthController> logger)
  {
        _authRepository = authRepository;
        _logger = logger;
  }

  public IActionResult ForgotPasswordBasic() => View();
  
  [HttpGet]
  public IActionResult LoginBasic() => View();
  
  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> LoginBasic(Auth auth)
  {
        try
        {
            //Validate input
            if (string.IsNullOrWhiteSpace(auth.username) || string.IsNullOrWhiteSpace(auth.password))
            {
                ModelState.AddModelError("", "Username/Email and Password are required.");
                return View();
            }
            //if (auth.username == null || auth.password == null)
            //{
            //    return View();
            //}

            var authResult = await _authRepository.LoginAsync(auth.username, auth.password);

            //if (authResult == null)
            //{
            //    return NotFound();
            //}

            if (authResult == null)
            {
                ModelState.AddModelError("", "Invalid username/email or password.");
                _logger.LogWarning("Failed login attempt for: {Username}", auth.username);
                return View();
            }

            TempData["userid"] = Convert.ToString(authResult.userid);
            TempData["groupid"] = Convert.ToString(authResult.groupid);
            TempData["firstname"] = Convert.ToString(authResult.firstname);
            TempData["lastname"] = Convert.ToString(authResult.lastname);
            TempData["stafftype"] = Convert.ToString(authResult.stafftype);
            TempData["designation"] = Convert.ToString(authResult.designation);
            TempData["mobile"] = Convert.ToString(authResult.mobile);
            TempData["emailid"] = Convert.ToString(authResult.emailid);
            TempData["password"] = Convert.ToString(authResult.password);
            TempData["createdby"] = Convert.ToString(authResult.createdby);
            TempData["updatedby"] = Convert.ToString(authResult.updatedby);
            TempData["SuccessMessage"] = null;
            // Keep TempData to ensure it persists for the redirect
            TempData.Keep("groupid");
            TempData.Keep("userid");

            _logger.LogInformation("User {Username} logged in successfully. GroupId: {GroupId}", auth.username, authResult.groupid);
            return RedirectToAction("Index", "Dashboards");
            //return View("Views/Dashboards");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for: {Username}", auth.username);
            ModelState.AddModelError("", "An error occurred during login. Please try again.");
            return View();
        }
    }
    //try
    //{
    //  // Validate input
    //  if (string.IsNullOrWhiteSpace(emailUsername) || string.IsNullOrWhiteSpace(password))
    //  {
    //    ModelState.AddModelError("", "Username/Email and Password are required.");
    //    return View();
    //  }

    //  // Validate credentials using repository
    //  var user = await _userRepository.ValidateCredentialsAsync(emailUsername, password);

    //  if (user == null)
    //  {
    //    ModelState.AddModelError("", "Invalid username/email or password.");
    //    _logger.LogWarning("Failed login attempt for: {Username}", emailUsername);
    //    return View();
    //  }

    //  // TODO: Implement session management or authentication cookies here
    //  // For example: await HttpContext.SignInAsync(...) or set session variables

    //  _logger.LogInformation("User {Username} logged in successfully.", user.Username);

    //  // Redirect to dashboard on successful login
    //  return RedirectToAction("Index", "Dashboards");
    //}
    //catch (Exception ex)
    //{
    //  _logger.LogError(ex, "Error during login for: {Username}", emailUsername);
    //  ModelState.AddModelError("", "An error occurred during login. Please try again.");
    //  return View();
    //}

  
  public IActionResult RegisterBasic() => View();

    /// <summary>
    /// Retrieves menu items for the logged-in user based on their group ID.
    /// This method can be called directly or used by the MenuDataFilter.
    /// </summary>
    /// <returns>List of Menu items for the user's group</returns>
    [HttpGet]
    public async Task<IActionResult> ShowMenu()
    {
        try
        {
            // Try to get groupid from TempData first, then from query/route if available
            var groupIdValue = TempData["groupid"]?.ToString() ?? Request.Query["groupId"].ToString();
            
            if (string.IsNullOrEmpty(groupIdValue) || !int.TryParse(groupIdValue, out int iGroupId))
            {
                _logger.LogWarning("ShowMenu called without valid groupid");
                return Json(new { success = false, message = "Group ID is required" });
            }

            // Keep TempData to ensure it persists for subsequent requests
            TempData.Keep("groupid");
            TempData.Keep("userid");

            List<Menu> menuResult = await _authRepository.AuthMenuAsync(iGroupId);

            if (menuResult == null || !menuResult.Any())
            {
                _logger.LogWarning("No menu items found for GroupId: {GroupId}", iGroupId);
                return Json(new { success = false, message = "No menu items found", data = new List<Menu>() });
            }

            // Store in ViewData for views that might need it
            ViewData["MenuData"] = menuResult;

            // Return JSON for AJAX calls, or the list for direct calls
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest" || Request.Query["format"].ToString() == "json")
            {
                return Json(new { success = true, data = menuResult });
            }

            // If called as a regular action, return the menu data
            // Note: This is typically handled by MenuDataFilter automatically
            return Json(new { success = true, data = menuResult });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading menu items");
            return Json(new { success = false, message = "An error occurred while loading menu items" });
        }
    }


}
