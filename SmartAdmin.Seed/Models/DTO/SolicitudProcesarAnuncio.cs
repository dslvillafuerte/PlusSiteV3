using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{
    public class SolicitudProcesarAnuncio
    {
        public string VendorId { get; set; }
        public string AdvertType { get; set; }
        public string AdvertTime { get; set; }
        public string AdvertText { get; set; }
        public bool IsActive { get; set; }
    }
}
