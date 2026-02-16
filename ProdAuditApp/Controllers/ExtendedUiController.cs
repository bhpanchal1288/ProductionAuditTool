using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.UI.Models;

namespace ProdAuditApp.UI.Controllers;

public class ExtendedUiController : Controller
{
  public IActionResult PerfectScrollbar() => View();
  public IActionResult TextDivider() => View();
}
