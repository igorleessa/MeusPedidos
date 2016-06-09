using ADODB;
using MPWebService.CORE;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace MPWebService.Skafe
{
    public class PostPrecoDA
    {
        public static PostPrecoBO GetPreco(string Codigo, Connection cn)
        {
            var RsPreco = new Recordset();
            var Retorno = new PostPrecoBO();
            

            try
            {
                RsPreco.Open(string.Format("SELECT * FROM Tabelas_de_Preços WHERE Código = '{0}'", Codigo),
                    cn, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
                if (!RsPreco.EOF)
                {
                    Retorno.nome = RsPreco.Fields["Descrição"].Value.ToString();
                    Retorno.tipo = "A";
                    Retorno.acrescimo = 0;
                    Retorno.desconto = null;
                    Retorno.excluido = false;
                }

                try
                {
                    SerializerFO Serializer = new SerializerFO();
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

                    var RespostaPost = responseString;

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
            return Retorno;
        }

        public static PostPrecoBO[] ListPreco(Connection cn)
        {
            var RsPreco = new Recordset();
            try
            {
                RsPreco.Open(string.Format("SELECT Código FROM Tabelas_de_Preços"), cn,
                    CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly);
                if (!RsPreco.EOF)
                {
                    var ListarPrecos = new PostPrecoBO[RsPreco.RecordCount];
                    for (int i = 0; i < RsPreco.RecordCount; RsPreco.MoveNext(), i++)
                    {
                        ListarPrecos[i] = GetPreco(RsPreco.Fields["Código"].Value.ToString(), cn);

                    }
                    RsPreco.Close();
                    return ListarPrecos;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PostPrecoBO[0];
        }


        public static PostClienteBO InsertIDPreco()
        {
            var RsPreco = new Recordset();
            var Retorno = new PostClienteBO();

            return Retorno;
        }

    }
}