using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{
    public class OrderDetailsItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string lineItemId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesOrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string sku { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string salesOrder { get; set; }
    }

    public class RespuestaDetallePedido
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
        public string totalOrder { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string orderStatus { get; set; }
        /// <summary>
        /// 
        /// </summary>
       public string customerIdentification { get; set; }
        public string customerAddress { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string email { get; set; }
        public List<OrderDetailsItem> orderDetails { get; set; }
    }

}
