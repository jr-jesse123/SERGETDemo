using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.Data;
using SERGETStore.Data.Contexto;
using SERGETStore.App.Extentions;
using SERGETStore.Business.Interfaces;
using SERGETStore.Data.Repository;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

var cfg = builder.Configuration;
var services = builder.Services;

var userConStr = cfg.GetConnectionString("UserContextConnection");
var sergetConStr = cfg.GetConnectionString("SergetContextConnection");

services.AddDbContext<UserDbContext>(options => options.UseSqlServer(userConStr));
services.AddDbContext<SERGETStoreAppContext>(options => options.UseSqlServer(sergetConStr));


services.AddAutoMapper(typeof(Program));

services.AddScoped<IProdutoRepository, ProdutoRepository>();
services.AddScoped<IFornecedorRepository, FornecedorRepository>();  
services.AddScoped<IEnderecoRepository, EnderecoRepository>();

    

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<UserDbContext>();



builder.Services.AddControllersWithViews(o =>
{
    o.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor((_, _) => "O valor preenchido é inválido para este campo.");
    o.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor((_) => "Este campo precisa ser preenchido.");
    o.ModelBindingMessageProvider.SetMissingKeyOrValueAccessor(() => "Este campo precisa ser preenchido");
    o.ModelBindingMessageProvider.SetMissingRequestBodyRequiredValueAccessor(() => "É necessário que o body na requisição ñao esteja vazio");
    o.ModelBindingMessageProvider.SetNonPropertyAttemptedValueIsInvalidAccessor((_) => "O valor preenchido é inválido para este campo.");
    o.ModelBindingMessageProvider.SetNonPropertyUnknownValueIsInvalidAccessor(() => "O valor preenchido é inválido para este campo.");
    o.ModelBindingMessageProvider.SetNonPropertyValueMustBeANumberAccessor(() => "O campo deve ser numérico");
    o.ModelBindingMessageProvider.SetUnknownValueIsInvalidAccessor(_ => "O valor preenchido é inválido para este campo");
    o.ModelBindingMessageProvider.SetValueIsInvalidAccessor((_) => "O valor preenchido é inválido para este campo.");
    o.ModelBindingMessageProvider.SetValueMustBeANumberAccessor((_) => "O campo deve ser numérico");
    o.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(_ => "Este campo precisa ser preenchido");
});


builder.Services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();



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


//TODO: DECLARAR ESTE ESCOPO
var defaultCulture = new CultureInfo("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture}
};

app.UseRequestLocalization(localizationOptions);



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
