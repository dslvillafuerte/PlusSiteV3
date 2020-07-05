#region Using

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SistemaPedidos.Models;
using SistemaPedidos.Services;
using SistemaPedidos.Utilidades;
using SistemaPedidos.Utils;
using SmartAdmin.Seed.Extensions;

#endregion

namespace SistemaPedidos.Controllers
{

    [Route("[controller]/[action]")]
    [Layout("_AuthLayout")]
    public class AccountController : Controller
    {

        public IConfiguration Configuration { get; }
        private readonly ILogger _logger;
        private readonly IZohoApis zohoApis;

        [TempData]
        public string ErrorMessage { get; set; }

        public AccountController(IConfiguration configuration, ILogger<AccountController> logger, IZohoApis zohoApis)
        {
            _logger = logger;

            Configuration = configuration;
            this.zohoApis = zohoApis;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var id = LoggerBase.ObtenerIdTransaccion();
            try
            {
                var respuesta = await zohoApis.Login(model.Email, model.Password);
                HttpContext.Session.SetString("IdEstablecimiento", respuesta.vendorId);
                LoggerBase.WriteLog("LoginController", id, "Iniciosesion", respuesta, TypeError.Info);
                this.TempData["Mensaje"] = $"{Mensaje.MensajeSatisfactorio}|{"Bienvenido"}";
                return RedirectToAction("Index", "Home");// RedirectToAction("Index", "Home");
                
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("LoginController", id, "Iniciosesion", ex, TypeError.Error);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(model);
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            try
            {
                HttpContext.Session.SetString("IdEstablecimiento", "");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Logout", LoggerBase.ObtenerIdTransaccion(), "SalirSesion", ex, TypeError.Error);
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                this.TempData["Mensaje"] = $"{Mensaje.MensajeSatisfactorio}|{"Bienvenido"}";
                return Redirect(returnUrl);
            }
            this.TempData["Mensaje"] = $"{Mensaje.MensajeSatisfactorio}|{"Bienvenido"}";
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}
