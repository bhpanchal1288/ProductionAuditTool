using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.UI.Models;

namespace ProdAuditApp.UI.Controllers;

public class DashboardsController : Controller
{
  public IActionResult Index()
  {
    // Keep TempData to ensure menu data can be loaded by MenuDataFilter
    TempData.Keep("groupid");
    TempData.Keep("userid");
    TempData.Keep("firstname");
    TempData.Keep("lastname");
    
    ViewData["Title"] = "Dashboard - Analytics";
    return View();
  }
}
