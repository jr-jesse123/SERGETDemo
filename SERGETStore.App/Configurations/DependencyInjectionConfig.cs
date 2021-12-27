using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using SERGETStore.App.Data;
using SERGETStore.Business.Interfaces;
using SERGETStore.Business.Notificacoes;
using SERGETStore.Data.Repository;
using System.Globalization;

namespace SERGETStore.App.Extentions
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            services.AddScoped<IFornecedorRepository, FornecedorRepository>();
            services.AddScoped<IEnderecoRepository, EnderecoRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, MoedaValidationAttributeAdapterProvider>();


            services.AddScoped<INotificador, Notificador>()
                .AddScoped<IFornecedorService, FornecedorService>()
                .AddScoped<IProdutoService, ProdutoService>();

            return services;

        }
    }


    public static class MvcConfig
    {
        public static IServiceCollection AddMVvcConfiguration(this IServiceCollection services)
        {

            services.AddControllersWithViews(o =>
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

            return services;
        }

        public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services, IConfiguration configuration)
        {

            var userConStr = configuration.GetConnectionString("UserContextConnection");

            services.AddDbContext<UserDbContext>(options => options.UseSqlServer(userConStr));
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultUI()
                .AddEntityFrameworkStores<UserDbContext>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            return services;
        }



        public static void UseGlobalizationConfig(this IApplicationBuilder app)
        {   
            var defaultCulture = new CultureInfo("pt-BR");
            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture }
            };
            app.UseRequestLocalization(localizationOptions);

        }
    }

}
