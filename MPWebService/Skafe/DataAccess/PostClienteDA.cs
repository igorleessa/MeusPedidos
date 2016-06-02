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

namespace MPWebService.Skafe
{
    public class PostClienteDA
    {
        public static PostClienteBO GetCliente(string Codigo, Connection cn)
        {
            var RsCliente = new Recordset();
            var Retorno = new PostClienteBO();

            try
            {
                RsCliente.Open(string.Format("select ContRazSoc,ContFant,ContCGC,ContIE,Suframa,Logradouro,numero,cep,Bairro,Municipios.Nome as Cidade , Municipios.Estado  from contas  inner join enderecos on contas.entrega = enderecos.enderecoID  inner join Municipios on Enderecos.MunicipioID = Municipios.MunicipioID where ContCliente = 1 and ContEmpresa =0 and ContInativo=0 and ContPessoaFis=0 and Código = '{0}'", Codigo),
                    cn, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
                if (!RsCliente.EOF)
                {
                    Retorno.razao_social = RsCliente.Fields["ContRazSoc"].Value.ToString();
                    Retorno.nome_fantasia = RsCliente.Fields["ContFant"].Value.ToString();
                    Retorno.tipo = "J";
                    string tratocnpj = RsCliente.Fields["ContCGC"].Value.ToString();

                    Regex digitsOnly = new Regex(@"[^\d]");
                    Retorno.cnpj = digitsOnly.Replace(tratocnpj, "");



                    Retorno.excluido = false;
                }

                try
                {
                    SerializerFO Serializer = new SerializerFO();
                    var request = (HttpWebRequest)WebRequest.Create("http://sandbox.meuspedidos.com.br:8080/api/v1/tabelas_preco/");

                    var data = Encoding.ASCII.GetBytes(Serializer.Serializador(Retorno));
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
            return Retorno;
        }
    }
}