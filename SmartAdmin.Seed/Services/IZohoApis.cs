using SistemaPedidos.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace SistemaPedidos.Services
{
    public interface IZohoApis
    {
        Task<RespuestaLoginUsuarioZoho> Login(string user , string password);
        Task<List<ProductoDto>> ObtenerProductosPorVendedor(string user);
        Task<string> EditarProducto(ProductoViewModel producto);
        Task<RespuestaDetallePedido> DetallePedido(string pedidoId);
        Task<bool> ProcesarPedido(SolicitudProcesarPedido solicitudProcesarPedido);
        Task<List<RespuestaPedidoActual>> ObtenerPedidos(string user, DateTime fecha);
        Task<List<AnuncioDto>> ObtenerAnunciosPorVendedor(string user);
        Task<bool> ProcesarAnuncio(SolicitudProcesarAnuncio anuncio);
    }
}
