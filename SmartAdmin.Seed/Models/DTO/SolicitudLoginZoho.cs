using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaPedidos.Models.DTO
{
    public class SolicitudLoginZoho
    {

        /// <summary>
        /// 
        /// </summary>
        public string key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool enabled { get; set; }

    }

    public class LoginUsuarioZohoo 
    {
        /// <summary>
        /// 
        /// </summary>
        public string userMail { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }

    }
}
