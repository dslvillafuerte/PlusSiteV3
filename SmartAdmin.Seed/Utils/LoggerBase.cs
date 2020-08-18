using System;
using System.Collections.Generic;
using System.Reflection;
using log4net;
using SistemaPedidos.Extensores;
using SistemaPedidos.Models.DTO;

namespace SistemaPedidos.Utilidades
{

    /// <summary>
    /// 
    /// </summary>
    public enum TypeError
    {
        Info,
        Warning,
        Error,

    }


    /// <summary>
    /// 
    /// </summary>
    public static class LoggerBase
    {



        #region Instancias

        /// <summary>
        /// Logger
        /// </summary>
        public static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #endregion Instancias

        #region Variables

        #region Uri

        public static string urlBase { get; set; }

        #endregion Uri

        public static List<AccionProceso> ListaAcciones { get; set; }
        public static string mensajePedidoProcesadoOk { get; internal set; }
        public static string mensajeNoExisteDetallePedido { get; internal set; }
        public static string mensajePedidoProcesadoError { get; internal set; }
        public static string EstadoPendiente { get; set; }

        #endregion Variables

        public static string ObtenerIdTransaccion() => Guid.NewGuid().ToString();


        public static void WriteLog(string metodo,string id,string url, object content, TypeError typeError, object request=null)
        {
            try
            {
                switch (typeError)
                {
                    case TypeError.Info:
                        Logger.Info($"[{metodo}]-[{id}]-[{url}]-[{content.Serializar()}],[{request.Serializar()}]");
                        break;
                    case TypeError.Warning:
                        Logger.Warn($"[{metodo}]-[{id}]-[{url}]-[{content.Serializar()}],[{request.Serializar()}]");
                        break;
                    case TypeError.Error:
                        Logger.Error($"[{metodo}]-[{id}]-[{url}]-[{content.Serializar()}],[{request.Serializar()}]");
                        break;
                }
            }
            catch
            {
            }
        }
    }

    
}