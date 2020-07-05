using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{

    public class SolicitudProcesarPedido
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Proceso { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Minutos { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Razon { get; set; }

        public DateTime Fecha { get; set; }

        public RespuestaDetallePedido DetallePedidoLista { get; set; }
    }
    
}
