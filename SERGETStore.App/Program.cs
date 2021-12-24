using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.Data;
using SERGETStore.Data.Contexto;
using SERGETStore.App.Extentions;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;
var services = builder.Services;

var userConStr = cfg.GetConnectionString("UserContextConnection");
var sergetConStr = cfg.GetConnectionString("SergetContextConnection");

services.AddDbContext<UserDbContext>(options => options.UseSqlServer(userConStr));
services.AddDbContext<SERGETStoreAppContext>(options => options.UseSqlServer(sergetConStr));
    

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
