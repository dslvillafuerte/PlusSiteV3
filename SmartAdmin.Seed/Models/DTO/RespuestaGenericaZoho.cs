using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{


    public class Details
    {

        /// <summary>
        /// 
        /// </summary>
        public string output { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string output_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
    }


    public class Root
    {
        /// <summary>
        /// 
        /// </summary>
        public string salesOrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string customerMobile { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int totalOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<string> orderDetails { get; set; }
    }



    public class RespuestaGenericaZoho
    {

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Details details { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
    }

    public class RespuestaLoginUsuarioZoho
    {

        public string vendorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendorName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendorUser { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string vendorPassword { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string atencionPromedio { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string direccion { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double latitud { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double longitud { get; set; }
    }

}
