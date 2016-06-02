using ADODB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using LibOrgm;
using System.Net;
using System.Text;
using System.IO;
using RestSharp;
using System.Text.RegularExpressions;
using MPWebService.CORE;
using MPWebService.Skafe;

namespace MPWebService
{
    /// <summary>
    /// Summary description for WebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void PostPreco(string JsonChamada)
        {
            var SQL = new LibOrgm.SQL();
            var cn = new ADODB.Connection();
            SerializerFO Serializer = new SerializerFO();

            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                SQL.AbrirConexao(cn);

                var retorno = PostPrecoFO.ListarPostPreco(JsonChamada, new PostPrecoWO(), cn);
                Context.Response.Write(Serializer.Serializador(retorno));
            }
            catch (Exception Ex)
            {
                Context.Response.Write(Ex.Message);
            }
            finally
            {
                SQL.FecharConexao(cn);
            }
        }




        //[WebMethod]
        //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        //public void GetPreco(string JsonChamada)
        //{
        //    SerializerFO Serializer = new SerializerFO();

        //    try
        //    {
        //        Context.Response.Clear();
        //        Context.Response.ContentType = "application/json";


        //        var client = new RestClient("http://sandbox.meuspedidos.com.br:8080/api/v1/tabelas_preco/");
        //        var request = new RestRequest(Method.GET);
        //        request.AddHeader("postman-token", "d7700f04-545a-e4de-186a-88ea9f9f9c5d");
        //        request.AddHeader("cache-control", "no-cache");
        //        request.AddHeader("content-type", "application/json");
        //        request.AddHeader("companytoken", "6e65c8c6-212a-11e6-8779-0a52011679b3");
        //        request.AddHeader("applicationtoken", "91bea5cc-212a-11e6-8779-0a52011679b3");
        //        IRestResponse response = client.Execute(request);

        //        var Retorno = GetPrecoFO.
        //        Context.Response.Write(response.Content);
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void Teste(string JsonChamada)
        {
            var SQL = new LibOrgm.SQL();
            var cn = new ADODB.Connection();
            SerializerFO Serializer = new SerializerFO();

            var RsPreco = new Recordset();
            var Retorno = new PostPrecoBO();

            try
            {
                Retorno.nome = "TESTE COTAÇÃO ÇÇÇ";
                Retorno.tipo = "A";
                Retorno.acrescimo = 1;
                Retorno.desconto = null;
                Retorno.excluido = false;

                try
                {
                    var request = (HttpWebRequest)WebRequest.Create("http://sandbox.meuspedidos.com.br:8080/api/v1/tabelas_preco/");

                    var data = Encoding.UTF8.GetBytes(Serializer.Serializador(Retorno));
                    request.Headers.Add("ApplicationToken", "91bea5cc-212a-11e6-8779-0a52011679b3");
                    request.Headers.Add("CompanyToken", "6e65c8c6-212a-11e6-8779-0a52011679b3");
                    request.Method = "POST";
                    request.ContentType = "application/json";

                    request.ContentLength = data.Length;

                    using (var stream = request.GetRequestStream())
                    {
                        stream.Write(data, 0, data.Length);
                    }

                    var response = (HttpWebResponse)request.GetResponse();
                    var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }

            finally
            {
                SQL.FecharConexao(cn);
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void teste(string JsonChamada)
        {
            SerializerFO Serializer = new SerializerFO();

            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";


                var Retorno = new PostClienteBO();

                Retorno.razao_social = "carlos";
                Retorno.nome_fantasia = "cae";
                Retorno.tipo = "J";
                string tratocnpj = "02.743.313\0001-65";

                Regex digitsOnly = new Regex(@"[^\d]");
                Retorno.cnpj = digitsOnly.Replace(tratocnpj, "");



                Retorno.excluido = false;


                Context.Response.Write(Serializer.Serializador(Retorno));
            }
            catch (Exception Ex)
            {
                Context.Response.Write(Ex.Message);
            }
        }
    }
}
