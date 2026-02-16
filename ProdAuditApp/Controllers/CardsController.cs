using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProdAuditApp.UI.Models;

namespace ProdAuditApp.UI.Controllers;

public class CardsController : Controller
{
  public IActionResult Basic() => View();
}
