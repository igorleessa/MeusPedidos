using ADODB;
using MPWebService.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MPWebService.Skafe
{
    public class GetClienteFO
    {
        public static GetClienteBO[] ObterGetCliente(string JsonChamada, GetClienteWO Chamada, Connection cn)
        {
            var Serializer = new SerializerFO();

            try
            {
                JavaScriptSerializer Js = new JavaScriptSerializer();
                GetClienteBO[] GetCliente = Js.Deserialize<GetClienteBO[]>(JsonChamada);

                var RespostaCliente = GetClienteDA.ExcluirCliente(GetCliente);

                return RespostaCliente;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}