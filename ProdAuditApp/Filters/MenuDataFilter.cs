using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using ProdAuditApp.Data.Repository.AuthRepository;

namespace ProdAuditApp.UI.Filters;

public class MenuDataFilter : IAsyncActionFilter
{
    private readonly IAuthRepository _authRepository;
    private readonly ITempDataDictionaryFactory _tempDataFactory;
    private readonly ILogger<MenuDataFilter>? _logger;

    public MenuDataFilter(
        IAuthRepository authRepository,
        ITempDataDictionaryFactory tempDataFactory,
        ILogger<MenuDataFilter>? logger = null)
    {
        _authRepository = authRepository;
        _tempDataFactory = tempDataFactory;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.Controller is Controller controller)
        {
            var tempData = _tempDataFactory.GetTempData(context.HttpContext);
            var groupIdValue = tempData?.Peek("groupid");

            if (groupIdValue != null && int.TryParse(Convert.ToString(groupIdValue), out var groupId))
            {
                // Avoid reloading if already set by another filter/action
                if (!controller.ViewData.ContainsKey("MenuData"))
                {
                    try
                    {
                        var menuItems = await _authRepository.AuthMenuAsync(groupId);
                        controller.ViewData["MenuData"] = menuItems;
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Unable to load menu for group {GroupId}", groupId);
                    }
                }

                // Preserve temp data keys for subsequent requests
                tempData?.Keep("groupid");
            }
        }

        await next();
    }
}

