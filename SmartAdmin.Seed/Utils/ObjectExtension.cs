using System.Collections.Specialized;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace SistemaPedidos.Extensores
{
    public static class ObjectExtension
    {
        public static object Serializar(this object str)
        {
			try
			{
				return str == null ? string.Empty : JsonConvert.SerializeObject(str);
			}
			catch (System.Exception)
			{

				return string.Empty;
			}
        }
    }
}