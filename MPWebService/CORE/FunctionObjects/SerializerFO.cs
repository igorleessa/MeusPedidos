using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MPWebService.CORE
{
    public class SerializerFO
    {
        public string Serializador(object Objeto)
        {
            string Json;
            try
            {

                MemoryStream Stream = new MemoryStream();
                DataContractJsonSerializer Serializador = new DataContractJsonSerializer(Objeto.GetType());

                Serializador.WriteObject(Stream, Objeto);

                Stream.Position = 0;
                StreamReader Leitor = new StreamReader(Stream);

                Json = Leitor.ReadToEnd();

            }
            catch
            {
                throw;
            }

            return Json;
        }

        public string MontarJson(JsonBO[] ObjetoJson)
        {
            string retorno = "{";
            string delimitador = "";

            try
            {
                foreach (JsonBO Elemento in ObjetoJson)
                {
                    switch (Elemento.Tipo)
                    {
                        case "adBoolean":
                            retorno += delimitador + "\"" + Elemento.Campo + "\":" + (Boolean.Parse(Elemento.Valor) ? "true" : "false");
                            break;
                        case "adLongVarChar":
                        case "adVarChar":
                        case "adDate":
                            retorno += delimitador + "\"" + Elemento.Campo + "\":\"" + Elemento.Valor + "\"";
                            break;
                        default:
                            retorno += "\"" + Elemento.Campo + "\":" + Elemento.Valor;
                            break;
                    }
                    delimitador = ",";
                }

                retorno += "}";
            }
            catch (Exception)
            {
                throw;
            }
            return retorno;
        }

        public object DeserializarObjetoJson(string Json, object Objeto)
        {

            try
            {

                MemoryStream Stream = new MemoryStream(Encoding.Unicode.GetBytes(Json));
                DataContractJsonSerializer Desserializador = new DataContractJsonSerializer(Objeto.GetType());

                Objeto = Desserializador.ReadObject(Stream);

            }
            catch
            {
                throw;
            }
            return Objeto;
        }

        public string GerarTokenAcesso()
        {
            string codigo = "";
            try
            {
                Guid g = Guid.NewGuid();
                string GuidString = Convert.ToBase64String(g.ToByteArray());
                GuidString = GuidString.Replace("=", "");
                GuidString = GuidString.Replace("+", "");
                GuidString = GuidString.Replace("/", "");
                codigo = GuidString;
            }
            catch (Exception)
            {
                throw;
            }
            return codigo;
        }
    }
}