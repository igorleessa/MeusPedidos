using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;

namespace MPWebService.Skafe
{
    public class GetClienteDA
    {
        public static GetClienteBO[] ExcluirCliente(GetClienteBO[] Json)
        {
            var Retorno = new GetClienteBO();
            
            for (int i = 0; i < Json.Length; i++)
            {
                Retorno.id = Json[i].id;
                Retorno.razao_social = "DESABILITADO";
                Retorno.nome_fantasia = "DESABILITADO";
                Retorno.tipo = "J";
                Retorno.Inscricao_estadual = "";
                Retorno.cnpj = "02743313000165";
                Retorno.bairro = "DESABILITADO";
                Retorno.suframa = "";
                Retorno.rua = "DESABILITADO";
                Retorno.cep = "26012480";
                Retorno.estado = "RJ";
                Retorno.cidade = "DESABILITADO";
                Retorno.excluido = false;

                var JsonString = new JavaScriptSerializer().Serialize(Retorno);

                var client = new RestClient("http://sandbox.meuspedidos.com.br:8080/api/v1/clientes/" + Retorno.id);
                var request = new RestRequest(Method.PUT);
                request.AddHeader("postman-token", "febae4cf-5ca0-12d8-f396-0c249256f660");
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("companytoken", "6e65c8c6-212a-11e6-8779-0a52011679b3");
                request.AddHeader("applicationtoken", "91bea5cc-212a-11e6-8779-0a52011679b3");
                request.AddParameter("application/json", JsonString, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);

            }

            return Json;

        }
    }
}