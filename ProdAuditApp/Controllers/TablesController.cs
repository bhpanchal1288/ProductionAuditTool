using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.UI.Models;

namespace ProdAuditApp.UI.Controllers;

public class TablesController : Controller
{
  public IActionResult Basic() => View();
}
