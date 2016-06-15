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
using System.Web.Script.Serialization;

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

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ExcluirPreco(string JsonChamada)
        {
            var SQL = new LibOrgm.SQL();
            var cn = new ADODB.Connection();
            SerializerFO Serializer = new SerializerFO();

            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                SQL.AbrirConexao(cn);

                var client = new RestClient("http://sandbox.meuspedidos.com.br:8080/api/v1/tabelas_preco/");
                var request = new RestRequest(Method.GET);
                request.AddHeader("postman-token", "d7700f04-545a-e4de-186a-88ea9f9f9c5d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("companytoken", "6e65c8c6-212a-11e6-8779-0a52011679b3");
                request.AddHeader("applicationtoken", "91bea5cc-212a-11e6-8779-0a52011679b3");
                IRestResponse response = client.Execute(request);

                Regex digitsOnly = new Regex(@"\t|\n|\r");
                JsonChamada = digitsOnly.Replace(response.Content, "");
                var RetornoGetPreco = GetPrecoFO.ObterGetPreco(JsonChamada, new GetPrecoWO(), cn);
                Context.Response.Write(Serializer.Serializador(JsonChamada));
            }
            catch (Exception)
            {
                throw;
            }
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void PostCliente(string JsonChamada)
        {
            var SQL = new LibOrgm.SQL();
            var cn = new ADODB.Connection();
            SerializerFO Serializer = new SerializerFO();

            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                SQL.AbrirConexao(cn);

                var retorno = PostClienteFO.ListarPostCliente(JsonChamada, new PostClienteWO(), cn);
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



        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public void ExcluirCliente(string JsonChamada)
        {
            var SQL = new LibOrgm.SQL();
            var cn = new ADODB.Connection();
            SerializerFO Serializer = new SerializerFO();

            try
            {
                Context.Response.Clear();
                Context.Response.ContentType = "application/json";
                SQL.AbrirConexao(cn);

                var client = new RestClient("http://sandbox.meuspedidos.com.br:8080/api/v1/clientes/");
                var request = new RestRequest(Method.GET);
                request.AddHeader("postman-token", "d7700f04-545a-e4de-186a-88ea9f9f9c5d");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("companytoken", "6e65c8c6-212a-11e6-8779-0a52011679b3");
                request.AddHeader("applicationtoken", "91bea5cc-212a-11e6-8779-0a52011679b3");
                IRestResponse response = client.Execute(request);

                Regex digitsOnly = new Regex(@"\t|\n|\r");
                JsonChamada = digitsOnly.Replace(response.Content, "");
                var RetornoGetPreco = GetClienteFO.ObterGetCliente(JsonChamada, new GetClienteWO(), cn);
                Context.Response.Write(Serializer.Serializador(JsonChamada));
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
