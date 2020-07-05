using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{
    public class AnuncioDto
    {
            public int advertId { get; set; }
            public string vendorId { get; set; }
            public int advertType { get; set; }
            public DateTime advertTime { get; set; }
            public string advertText { get; set; }
            public bool isActive { get; set; }
    }
}
