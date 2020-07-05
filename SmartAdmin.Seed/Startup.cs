#region Using

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SistemaPedidos.Models.DTO;
using SistemaPedidos.Services;
using SistemaPedidos.Utilidades;
using System;
using System.Collections.Generic;
using System.Globalization;

// ReSharper disable UnusedMember.Global
// ReSharper disable once ClassNeverInstantiated.Global

#endregion

namespace SmartAdmin.Seed
{
    /// <summary>
    /// Defines the startup instance used by the web host.
    /// </summary>

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            LoggerBase.urlBase = Configuration.GetSection("urlBase").Value;
            LoggerBase.mensajePedidoProcesadoOk = Configuration.GetSection("mensajePedidoProcesadoOk").Value;
            LoggerBase.mensajePedidoProcesadoError = Configuration.GetSection("mensajePedidoProcesadoError").Value;
            LoggerBase.mensajeNoExisteDetallePedido = Configuration.GetSection("mensajeNoExisteDetallePedido").Value;
            LoggerBase.ListaAcciones = Configuration.GetSection("listaAcciones").Get<List<AccionProceso>>();
            LoggerBase.EstadoPendiente = Configuration.GetSection("EstadoPendiente").Value;

            var TiempoVidaCookie = Convert.ToDouble(Configuration.GetSection("TiempoVidaCookie").Value);
            services.AddMvc();
            services.AddScoped<IZohoApis, ServicioZohoApis>();
            services.AddMemoryCache();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(TiempoVidaCookie + 500);//You can set Time
            });
            services.AddDistributedMemoryCache();
            services.AddResponseCaching();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var defaultCulture = new CultureInfo("es-EC");
            defaultCulture.NumberFormat.NumberDecimalSeparator = ".";
            defaultCulture.NumberFormat.CurrencyDecimalSeparator = ".";
            //defaultCulture.DateTimeFormat = DateTimeFormatInfo.CurrentInfo;
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = new List<CultureInfo> { defaultCulture },
                SupportedUICultures = new List<CultureInfo> { defaultCulture },
                FallBackToParentCultures = false,
                FallBackToParentUICultures = false,
                RequestCultureProviders = new List<IRequestCultureProvider> { }
            });
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }


            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            app.UseResponseCaching();
        }
    }
}
