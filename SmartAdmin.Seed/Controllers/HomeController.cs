#region Using

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SistemaPedidos.Models;
using SistemaPedidos.Models.DTO;
using SistemaPedidos.Services;
using SistemaPedidos.Utilidades;
using SistemaPedidos.Utils;

#endregion

namespace SistemaPedidos.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }

        private readonly IZohoApis zohoApis;

        public HomeController(IConfiguration configuration, IZohoApis zohoApis)
        {
            Configuration = configuration;
            this.zohoApis = zohoApis;
        }

        #region Pedidos Actuales

        public IActionResult Index()
        {
            var usuario = HttpContext.Session.GetString("IdEstablecimiento");
            LoggerBase.WriteLog("Home-Index-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> PedidosActuales()
        {
            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-PedidosActuales-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }
                var listaPedidosActuales = await zohoApis.ObtenerPedidos(usuario, DateTime.Now.Date);



                return Json(new
                {
                    estado = true,
                    procesar = listaPedidosActuales
                    .Where(x => x.EstadoPedido.ToUpper().Trim() == LoggerBase.EstadoPendiente.ToUpper().Trim()).Count(),
                    lista = listaPedidosActuales.Select(x =>
                    {
                        x.date = $"{x.date:yyyy-MM-dd}";
                        x.valorEnvio = x.valorEnvio ?? 0.00;
                        return x;
                    }),
                    sesionExpirada,
                    mensaje = string.Empty,
                });
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-PedidosActuales", "Usuario", "PedidosActuales", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    lista = string.Empty,
                    mensaje = ex.Message,
                });
            }
        }


        [HttpPost]
        public async Task<JsonResult> ProcesarPedidoEnvio(string id,string tipo)
        {
            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarPedidoEnvio-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                var pedido = new SolicitudProcesarPedido
                {
                    Id = id,
                    Proceso = tipo,
                };

                var respuestaPedido = await zohoApis.ProcesarPedido(pedido);

                return Json(new
                {
                    estado = respuestaPedido,
                    sesionExpirada,
                    mensaje = string.Empty,
                });



            }
            catch (Exception ex)
            {

                LoggerBase.WriteLog("Home-ProcesarPedidoEnvio", "Usuario", "ProcesarPedidoEnvio", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    lista = string.Empty,
                    mensaje = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcesarPedido(SolicitudProcesarPedido solicitudProcesarPedido)
        {
            try
            {
                RespuestaDetallePedido ListaDetallePedido = new RespuestaDetallePedido();
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarPedido-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                    return RedirectToAction("Login", "Account");
                }

                switch (solicitudProcesarPedido.Proceso)
                {
                    case "Aceptar":
                        solicitudProcesarPedido.Razon = string.Empty;
                        solicitudProcesarPedido.Minutos = 0;
                        break;
                    case "AceptarAtraso":
                        if (solicitudProcesarPedido.Minutos < 1 || string.IsNullOrWhiteSpace(solicitudProcesarPedido.Razon))
                        {
                            ModelState.AddModelError(string.Empty, "Debe completar el formulario.");
                            ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre", solicitudProcesarPedido.Proceso);

                            var detallePedido = await zohoApis.DetallePedido(solicitudProcesarPedido.Id);

                            if (detallePedido == null || detallePedido.orderDetails.Count == 0)
                            {
                                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                                return RedirectToAction("Index", "Home");
                            }

                            solicitudProcesarPedido.DetallePedidoLista = detallePedido;
                            return View(solicitudProcesarPedido);
                        }
                        break;
                    case "Negar":
                        if (string.IsNullOrWhiteSpace(solicitudProcesarPedido.Razon))
                        {
                            ModelState.AddModelError(string.Empty, "Debe completar el formulario.");
                            ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre", solicitudProcesarPedido.Proceso);
                            var detallePedido = await zohoApis.DetallePedido(solicitudProcesarPedido.Id);
                            if (detallePedido == null || detallePedido.orderDetails.Count == 0)
                            {
                                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                                return RedirectToAction("Index", "Home");
                            }
                            solicitudProcesarPedido.DetallePedidoLista = detallePedido;
                            return View(solicitudProcesarPedido);
                        }
                        break;
                }

                var pedido = await zohoApis.ProcesarPedido(solicitudProcesarPedido);
                if (pedido)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.MensajeSatisfactorio}|{LoggerBase.mensajePedidoProcesadoOk}";
                    return RedirectToAction("Index", "Home");// RedirectToAction("Index", "Home");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{LoggerBase.mensajePedidoProcesadoError}";
                ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre", solicitudProcesarPedido.Proceso);
                var detallePedidoActual = await zohoApis.DetallePedido(solicitudProcesarPedido.Id);

                var detalle = await zohoApis.DetallePedido(solicitudProcesarPedido.Id);

                if (detalle == null || detalle.orderDetails.Count == 0)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                    return RedirectToAction("Index", "Home");
                }

                solicitudProcesarPedido.DetallePedidoLista = detalle;
                return View(solicitudProcesarPedido);
            }
            catch (Exception ex)
            {

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProcesarPedido(string id)
        {
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarPedido-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                    return RedirectToAction("Login", "Account");
                }
                ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre");
                var detalle = await zohoApis.DetallePedido(id);

                if (detalle == null || detalle.orderDetails.Count == 0)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                    return RedirectToAction("Index", "Home");
                }


                return View(new SolicitudProcesarPedido { Id = id, DetallePedidoLista = detalle });
            }
            catch (Exception ex)
            {

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProcesarPedidoVer(string id)
        {
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarPedido-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                    return RedirectToAction("Login", "Account");
                }
                ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre");
                var detalle = await zohoApis.DetallePedido(id);

                if (detalle == null || detalle.orderDetails.Count == 0)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                    return RedirectToAction("Index", "Home");
                }
                 

                return View(new SolicitudProcesarPedido { Id = id, DetallePedidoLista = detalle });
            }
            catch (Exception ex)
            {

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Reportes

        public IActionResult PanelInformacion()
        {
            var usuario = HttpContext.Session.GetString("IdEstablecimiento");
            LoggerBase.WriteLog("Home-Index-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                return RedirectToAction("Login", "Account");
            }
            return View(new Models.DTO.RespuestaLoginUsuarioZoho { vendorId = usuario });
        }

        #endregion


        #region Productos

        [HttpGet]
        public IActionResult Productos()
        {
            var usuario = HttpContext.Session.GetString("IdEstablecimiento");
            LoggerBase.WriteLog("Home-Productos-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                return RedirectToAction("Login", "Account");
            }
            return View(new SolicitudProcesarAnuncio());
        }


        [HttpGet]
        public async Task<JsonResult> ObtenerProductos()
        {

            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ObtenerProductos-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                var listaProductos = await zohoApis.ObtenerProductosPorVendedor(usuario);

                return Json(new
                {
                    estado = true,
                    lista = listaProductos,
                    sesionExpirada,
                    mensaje = string.Empty,
                });
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-ObtenerProductos", "Usuario", "ObtenerAnunciosPorVendedorAnuncios", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    lista = string.Empty,
                    mensaje = ex.Message,
                });
            }
        }


        [HttpGet]
        public async Task<IActionResult> EditarProducto(string id)
        {
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ObtenerProducto-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                var listaProductos = await zohoApis.ObtenerProductosPorVendedor(usuario);

                var producto = listaProductos.Where(x => x.itemId.Equals(id)).FirstOrDefault();

                if (producto == null)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró el producto seleccionado"}";
                    return RedirectToAction("Productos", "Home");
                }

                return View(new ProductoViewModel
                {
                    Category=producto.category,
                    CommentsForAgent=producto.commentsForAgent,
                    IsActive=producto.isActive,
                    ItemDescription=producto.itemDescription,
                    ItemId=producto.itemId,
                    Price=producto.price,
                    SKU=producto.sku,
                    TaxPercent=producto.taxPercent,
                    VendorId=producto.vendorId,
                    VendorSKU=producto.vendorSku,
                });
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-ObtenerProducto", "Usuario", "ObtenerAnunciosPorVendedorAnuncios", ex.Message, TypeError.Error);
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Productos", "Home");
            }
        }


        [HttpPost]
        public async Task<IActionResult> EditarProducto(ProductoViewModel producto)
        {

            try
            {
                
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-EditarProducto-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                    return RedirectToAction("Login", "Account");
                }

                var pedido = await zohoApis.EditarProducto(producto);
                if (pedido.Equals("OK",StringComparison.InvariantCultureIgnoreCase))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.MensajeSatisfactorio}|{"El producto se actualizó correctamente"}";
                    return RedirectToAction("Productos", "Home");// RedirectToAction("Index", "Home");
                }

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"El producto no pudo ser editado"}";
                return View(producto);
            }
            catch (Exception ex)
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Productos", "Home");
            }
        }


        #endregion


        #region Anuncios





        [HttpGet]
        public IActionResult Anuncios()
        {
            var usuario = HttpContext.Session.GetString("IdEstablecimiento");
            LoggerBase.WriteLog("Home-PedidosHistoricos-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                return RedirectToAction("Login", "Account");
            }
            return View(new SolicitudProcesarAnuncio());
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerAnunciosPorVendedorAnuncios()
        {

            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ObtenerAnunciosPorVendedorAnuncios-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                var listaAnuncios = await zohoApis.ObtenerAnunciosPorVendedor(usuario);

                listaAnuncios = listaAnuncios.OrderByDescending(x => x.isActive).ThenByDescending(x=>x.advertTime).ThenBy(x=>x.advertType).ToList();

                return Json(new
                {
                    estado = true,
                    lista = listaAnuncios,
                    sesionExpirada,
                    mensaje = string.Empty,
                });
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-ObtenerAnunciosPorVendedorAnuncios", "Usuario", "ObtenerAnunciosPorVendedorAnuncios", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    lista = string.Empty,
                    mensaje = ex.Message,
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ProcesarAnuncio(SolicitudProcesarAnuncio anuncio)
        {
            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarAnuncio-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                anuncio.VendorId = usuario;
                anuncio.IsActive = true;
                var respuestaAnuncio = await zohoApis.ProcesarAnuncio(anuncio);

                return Json(new
                {
                    estado = respuestaAnuncio,
                    sesionExpirada,
                    mensaje = string.Empty,
                });


            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-ProcesarAnuncio", "Usuario", "ProcesarAnuncio", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    mensaje = ex.Message,
                });
            }
        }

        #endregion

        #region Pedidos Historicos

        [HttpGet]
        public IActionResult PedidosHistoricos(DateTime? fecha)
        {
            var usuario = HttpContext.Session.GetString("IdEstablecimiento");
            LoggerBase.WriteLog("Home-PedidosHistoricos-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                return RedirectToAction("Login", "Account");
            }
            return View(new PedidosHistoricosViewModel { FechaInicio = fecha ?? DateTime.Now.Date });
        }

        [HttpPost]
        public async Task<JsonResult> PedidosHistoricos(DateTime fecha)
        {

            var sesionExpirada = false;
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-PedidosHistoricos-POST", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    sesionExpirada = true;
                    throw new Exception("IdEstablecimiento no encontrado");
                }

                var listaPedidosActuales = await zohoApis.ObtenerPedidos(usuario, fecha.Date);

                return Json(new
                {
                    estado = true,
                    lista = listaPedidosActuales.Select(x =>
                    {
                        x.date = $"{x.date:yyyy-MM-dd}";
                        x.valorEnvio = x.valorEnvio ?? 0.00;
                        return x;
                    }),
                    sesionExpirada,
                    mensaje = string.Empty,
                });
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog("Home-PedidosActuales", "Usuario", "PedidosActuales", ex.Message, TypeError.Error);
                return Json(new
                {
                    estado = false,
                    sesionExpirada,
                    lista = string.Empty,
                    mensaje = ex.Message,
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ProcesarPedidoHistorico(string id, DateTime fecha)
        {
            try
            {
                var usuario = HttpContext.Session.GetString("IdEstablecimiento");
                LoggerBase.WriteLog("Home-ProcesarPedido-GET", "Usuario", "IdEstablecimiento", usuario, TypeError.Info);
                if (string.IsNullOrEmpty(usuario))
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"Su sesión ha expirado"}";
                    return RedirectToAction("Login", "Account");
                }
                ViewData["IdAccion"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(LoggerBase.ListaAcciones, "Accion", "AccionNombre");
                var detalle = await zohoApis.DetallePedido(id);

                if (detalle == null || detalle.orderDetails.Count == 0)
                {
                    this.TempData["Mensaje"] = $"{Mensaje.Error}|{"No se encontró detalle del pedido seleccionado"}";
                    return RedirectToAction("Index", "Home");
                }


                return View(new SolicitudProcesarPedido { Id = id, DetallePedidoLista = detalle, Fecha = fecha });
            }
            catch (Exception ex)
            {

                this.TempData["Mensaje"] = $"{Mensaje.Error}|{ex.Message}";
                return RedirectToAction("Index", "Home");
            }
        } 

        #endregion
       
        public IActionResult Error() => View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}
