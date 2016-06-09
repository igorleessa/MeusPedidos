using ADODB;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace MPWebService.Skafe
{
    public class GetPrecoDA
    {

        public static GetPrecoBO[] ExcluirPreco(GetPrecoBO[] Json)
        {
            var Retorno = new GetPrecoBO();

            for (int i = 0; i < Json.Length; i++)
            {
                Retorno.id = Json[i].id;
                Retorno.nome = "DESATIVADO";
                Retorno.tipo = Json[i].tipo;
                Retorno.acrescimo = 1;
                Retorno.desconto = Json[i].desconto;
                Retorno.excluido = true;

                var JsonString = new JavaScriptSerializer().Serialize(Retorno);

                var client = new RestClient("http://sandbox.meuspedidos.com.br:8080/api/v1/tabelas_preco/" + Retorno.id);
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



        public static GetPrecoBO GetPreco(GetPrecoWO Preco, Connection cn)
        {
            var RsGetPreco = new Recordset();
            var Retorno = new GetPrecoBO();

            try
            {
                RsGetPreco.Open(string.Format("SELECT * FROM Tabelas_de_Preços WHERE Descrição = '{0}'", Preco.nome),
                    cn, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
                if (!RsGetPreco.EOF)
                {
                    if (!string.IsNullOrEmpty(RsGetPreco.Fields["Descrição"].Value.ToString()))
                    {
                        Retorno.nome = RsGetPreco.Fields["Descrição"].Value.ToString();
                        if (Retorno.nome != Preco.nome)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return Retorno;
        }
    }
}