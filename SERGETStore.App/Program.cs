using Microsoft.EntityFrameworkCore;
using SERGETStore.Data.Contexto;
using SERGETStore.App.Extentions;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;
var services = builder.Services;


var sergetConStr = cfg.GetConnectionString("SergetContextConnection");

services.AddDbContext<SERGETStoreAppContext>(options => options.UseSqlServer(sergetConStr));
services.AddAutoMapper(typeof(Program));
services.AddMVvcConfiguration();
services.ResolveDependencies();
services.AddIdentityConfiguration(cfg);
services.AddDatabaseDeveloperPageExceptionFilter();


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
app.UseGlobalizationConfig();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
