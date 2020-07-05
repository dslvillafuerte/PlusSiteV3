using Newtonsoft.Json;
using SistemaPedidos.Models.DTO;
using SistemaPedidos.Utilidades;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;

namespace SistemaPedidos.Services
{
    public class ServicioZohoApis : IZohoApis
    {
        public HttpClient _clienteZoho = new HttpClient();

        

        public async Task<RespuestaLoginUsuarioZoho> Login(string user, string password)
        {
            var transaccion =LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo ="Login";
            try
            {
                    var request = JsonConvert.SerializeObject(new LoginUsuarioZohoo { password = password, userMail = user });
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    LoggerBase.WriteLog(nombreMetodo, transaccion,LoggerBase.urlBase,content,TypeError.Info,request);
                    var response = await _clienteZoho.PostAsync($"{LoggerBase.urlBase}api/vendor/LoginVendor" , content);
                    if (response.StatusCode==System.Net.HttpStatusCode.NoContent)
                    {
                        throw new Exception("Usuario o contaseña incorrecta");
                    }
                    var resultado = await response.Content.ReadAsStringAsync();
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                    var usuario = JsonConvert.DeserializeObject<RespuestaLoginUsuarioZoho>(resultado);
                    return usuario;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ProductoDto>> ObtenerProductosPorVendedor(string user)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "ObtenerProductosPorVendedor";
            try
            {
                var request = $"{LoggerBase.urlBase}/api/vendor/GetItemsByVendor?vendorId={user}";
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, null, TypeError.Info, request);
                var response = await _clienteZoho.GetAsync(request);
                var resultado = await response.Content.ReadAsStringAsync();
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                var listaProducto = JsonConvert.DeserializeObject<List<ProductoDto>>(resultado);
                return listaProducto;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> EditarProducto(ProductoViewModel producto)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "EditarProducto";
            try
            {
                var request = JsonConvert.SerializeObject(producto);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, content, TypeError.Info, request);
                var response = await _clienteZoho.PostAsync($"{LoggerBase.urlBase}/api/vendor/PostItem", content);
                var resultado = await response.Content.ReadAsStringAsync();
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                return resultado;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }

        public async Task<RespuestaDetallePedido> DetallePedido(string pedidoId)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "DetallePedido";
            try
            {
                    var request = $"{LoggerBase.urlBase}api/orders/GetOrder?Id={pedidoId}";
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, null, TypeError.Info,request);
                    var response = await _clienteZoho.GetAsync(request);
                    var resultado = await response.Content.ReadAsStringAsync();
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                    var listaPedidos = JsonConvert.DeserializeObject<RespuestaDetallePedido>(resultado);
                    return listaPedidos;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<RespuestaPedidoActual>> ObtenerPedidos(string user, DateTime fecha)
        {
            var transaccion =LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "ObtenerPedidos";
            try
            {
                    var request = $"{LoggerBase.urlBase}api/orders/GetOrdersByVendor?vendorId={user}&date={fecha:yyyy-MM-dd}";
                    LoggerBase.WriteLog(nombreMetodo, transaccion,LoggerBase.urlBase,null,TypeError.Info,request);
                    var response = await _clienteZoho.GetAsync(request);
                    var resultado = await response.Content.ReadAsStringAsync();
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                    var listaPedidos = JsonConvert.DeserializeObject<List<RespuestaPedidoActual>>(resultado);
                    return listaPedidos;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> ProcesarPedido(SolicitudProcesarPedido solicitudProcesarPedido)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "ProcesarPedido";
            try
            {
                    var request = JsonConvert.SerializeObject(solicitudProcesarPedido);
                    var content = new StringContent(request, Encoding.UTF8, "application/json");
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, content, TypeError.Info,request);
                    var response = await _clienteZoho.PostAsync($"{LoggerBase.urlBase}api/orders/WorkOrder", content);
                    var resultado = await response.Content.ReadAsStringAsync();
                    LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                    var respuesta = JsonConvert.DeserializeObject<bool>(resultado);
                    return respuesta;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> ProcesarAnuncio(SolicitudProcesarAnuncio anuncio)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "ProcesarAnuncio";
            try
            {
                var request = JsonConvert.SerializeObject(anuncio);
                var content = new StringContent(request, Encoding.UTF8, "application/json");
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, content, TypeError.Info, request);
                var response = await _clienteZoho.PostAsync($"{LoggerBase.urlBase}api/vendor/PostAdvert", content);
                var resultado = await response.Content.ReadAsStringAsync();
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                var respuesta = JsonConvert.DeserializeObject<bool>(resultado);
                return respuesta;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AnuncioDto>> ObtenerAnunciosPorVendedor(string user)
        {
            var transaccion = LoggerBase.ObtenerIdTransaccion();
            var nombreMetodo = "ObtenerAnunciosPorVendedor";
            try
            {
                var request = $"{LoggerBase.urlBase}/api/vendor/GetAdvertsByVendor?vendorId={user}";
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, null, TypeError.Info, request);
                var response = await _clienteZoho.GetAsync(request);
                var resultado = await response.Content.ReadAsStringAsync();
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, resultado, TypeError.Info);
                var listaAnuncios = JsonConvert.DeserializeObject<List<AnuncioDto>>(resultado);
                return listaAnuncios;
            }
            catch (Exception ex)
            {
                LoggerBase.WriteLog(nombreMetodo, transaccion, LoggerBase.urlBase, ex, TypeError.Error);
                throw new Exception(ex.Message);
            }
        }
    }
}
