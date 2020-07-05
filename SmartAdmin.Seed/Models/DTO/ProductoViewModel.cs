
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{
    public class ProductoViewModel
    {
        public string ItemId { get; set; }

        
        [MaxLength(50, ErrorMessage = "La descripción no puede tener más de 50 caracteres")]
        public string ItemDescription { get; set; }
        public string VendorId { get; set; }

        [MaxLength(50, ErrorMessage = "El SKU no puede tener más de 50 caracteres")]
        public string SKU { get; set; }

        
        [MaxLength(50, ErrorMessage = "El vendedor SKU no puede tener más de 50 caracteres")]
        public string VendorSKU { get; set; }

        
        [MaxLength(50, ErrorMessage = "El categoría no puede tener más de 50 caracteres")]
        public string Category { get; set; }

        
        [DataType(DataType.Currency)]
        public float Price { get; set; }

        
        [DataType(DataType.Currency)]
        public float TaxPercent { get; set; }

        
        [MaxLength(50, ErrorMessage = "El comentario para el agente no puede tener más de 50 caracteres")]
        public string CommentsForAgent { get; set; }
        public bool IsActive { get; set; }
    }
}
