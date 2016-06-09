using ADODB;
using MPWebService.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MPWebService.Skafe
{
    public class GetPrecoFO
    {
        public static GetPrecoBO[] ObterGetPreco(string JsonChamada, GetPrecoWO Chamada, Connection cn)
        {
            var Serializer = new SerializerFO();
            
            try
            {
                JavaScriptSerializer Js = new JavaScriptSerializer();
                GetPrecoBO[] GetPreco = Js.Deserialize<GetPrecoBO[]>(JsonChamada);

                var RespostaPreco = GetPrecoDA.ExcluirPreco(GetPreco);

                return RespostaPreco;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}