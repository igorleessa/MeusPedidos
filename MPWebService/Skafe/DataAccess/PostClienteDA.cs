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
                RsCliente.Open(string.Format("select substring(ContRazSoc,1,100) as RazaoSocial,substring(contas.ContaID +' - '+ ContFant,1,100) as Fantasia,substring(ContCGC,1,18) as cnpj,substring(ContIE,1,30) as IE,substring(Suframa,1,20) as suframatxt,substring(Logradouro +' , '+ numero,1,100) as rua,substring(cep,1,9) as ceptxt,substring(Bairro,1,30) as bairrotxt,substring(Municipios.Nome,1,50) as Cidade , substring(Municipios.Estado,1,2) as estadotxt  from contas  inner join enderecos on contas.entrega = enderecos.enderecoID  inner join Municipios on Enderecos.MunicipioID = Municipios.MunicipioID where ContCliente = 1 and ContEmpresa =0 and ContInativo=0 and ContPessoaFis=0 and Municipios.Estado not like'ex%'  and contas.contaID = '{0}'", Codigo),
                    cn, CursorTypeEnum.adOpenForwardOnly, LockTypeEnum.adLockReadOnly);
                if (!RsCliente.EOF)
                {

                    Retorno.razao_social = RsCliente.Fields["RazaoSocial"].Value.ToString();
                    Retorno.nome_fantasia = RsCliente.Fields["Fantasia"].Value.ToString();
                    Retorno.tipo = "J";
                    string tratocnpj = RsCliente.Fields["cnpj"].Value.ToString();
                    Regex digitsOnly = new Regex(@"[^\d]");
                    Retorno.Inscricao_estadual = RsCliente.Fields["ie"].Value.ToString();
                    Retorno.cnpj = digitsOnly.Replace(tratocnpj, "");
                    Retorno.bairro = RsCliente.Fields["bairrotxt"].Value.ToString();
                    Retorno.suframa = RsCliente.Fields["suframatxt"].Value.ToString();
                    Retorno.rua = RsCliente.Fields["rua"].Value.ToString();
                    Retorno.cep = RsCliente.Fields["ceptxt"].Value.ToString();
                    Retorno.estado = RsCliente.Fields["estadotxt"].Value.ToString();
                    Retorno.cidade = RsCliente.Fields["cidade"].Value.ToString();
                    Retorno.excluido = false;
                }

                try
                {
                    SerializerFO Serializer = new SerializerFO();
                    var request = (HttpWebRequest)WebRequest.Create("http://sandbox.meuspedidos.com.br:8080/api/v1/clientes/");

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
        public static PostClienteBO[] ListCliente(Connection cn)
        {
            var RsCliente = new Recordset();
            try
            {
                RsCliente.Open(string.Format("select contas.ContaID from contas inner join enderecos on contas.entrega = enderecos.enderecoID  inner join Municipios on Enderecos.MunicipioID = Municipios.MunicipioID where ContCliente = 1 and ContEmpresa = 0 and ContInativo = 0 and ContPessoaFis = 0 and Municipios.Estado not like'ex%'"), cn,
                    CursorTypeEnum.adOpenStatic, LockTypeEnum.adLockReadOnly);
                if (!RsCliente.EOF)
                {
                    var ListarPrecos = new PostClienteBO[RsCliente.RecordCount];
                    for (int i = 0; i < RsCliente.RecordCount; RsCliente.MoveNext(), i++)
                    {
                        ListarPrecos[i] = GetCliente(RsCliente.Fields["ContaID"].Value.ToString(), cn);

                    }
                    RsCliente.Close();
                    return ListarPrecos;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new PostClienteBO[0];
        }
    }
}