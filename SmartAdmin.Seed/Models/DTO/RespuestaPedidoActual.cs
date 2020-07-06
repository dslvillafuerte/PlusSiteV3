using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{


    public class RespuestaPedidoActual
    {
        [JsonProperty("salesOrderId")]
        public string salesorder_id { get; set; }

        [JsonProperty("customerName")]
        public string customer_name { get; set; }

        [JsonProperty("orderNumber")]
        public string salesorder_number { get; set; }

        [JsonProperty("totalOrder")]
        public double total { get; set; }

        [JsonProperty("shippingValue")]
        public double? valorEnvio { get; set; }

        [JsonProperty("paymentCharge")]
        public double? paymentCharge { get; set; }
        

        [JsonProperty("orderDate")]
        public string date { get; set; }

        [JsonProperty("customerMobile")]
        public string Phone { get; set; }

        [JsonProperty("orderStatus")]
        public string EstadoPedido { get; set; }

        public string dispatchAproxTime { get; set; }

        public string orderDispatch { get; set; }
        public string dispatchMinCount { get; set; }

    }

}
