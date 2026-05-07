using ProdAuditApp.Data.DataAccess;
using ProdAuditApp.Data.Repository.AuthRepository;
using ProdAuditApp.Data.Repository.GroupRepository;
using ProdAuditApp.Data.Repository.SubGroupRepository;
using ProdAuditApp.Data.Repository.ClientRepository;
using ProdAuditApp.Data.Repository.ProductionHouseRepository;
using ProdAuditApp.Data.Repository.VendorRepository;
using ProdAuditApp.Data.Repository.MasterConfigRepository;
using ProdAuditApp.Data.Repository.DropdownRepository;
using ProdAuditApp.Data.Repository.UserGroupRepository;
using ProdAuditApp.Data.Repository.UserRepository;
using ProdAuditApp.Data.Repository.ProjectsRepository;
using ProdAuditApp.Data.Repository.MappingRepository;
using ProdAuditApp.Data.Repository.BudgetRepository;
using ProdAuditApp.Data.Repository.LocationBudgetRepository;
using ProdAuditApp.Data.Repository.ContractsRepository;
using ProdAuditApp.UI.Filters;
using ProdAuditApp.UI.Middleware;
using ProdAuditApp.Data.Repository.CallSheetRepository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Register MenuDataFilter globally to load menu data for all actions
    options.Filters.Add<MenuDataFilter>();
});

// Register Data Access Layer
builder.Services.AddScoped<ISqlDataAccess, SqlDataAccess>();

// Register Repositories
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IMasterConfigService, MasterConfigService>();
builder.Services.AddScoped<IDropdownRepository, DropdownRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<ISubGroupRepository, SubGroupRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IProductionHouseRepository, ProductionHouseRepository>();
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IUserGroupRepository, UserGroupRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectsRepository, ProjectsRepository>();
builder.Services.AddScoped<IMappingRepository, MappingRepository>();
builder.Services.AddScoped<IBudgetRepository, BudgetRepository>();
builder.Services.AddScoped<ILocationBudgetRepository, LocationBudgetRepository>();
builder.Services.AddScoped<IContractsRepository, ContractsRepository>();
builder.Services.AddScoped<ICallSheetRepository, CallSheetRepository>();


// Register Filters
builder.Services.AddScoped<MenuDataFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=LoginBasic}");///{id?}");

app.Run();
