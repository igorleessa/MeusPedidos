using ADODB;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

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
                RsCliente.Open(string.Format("select substring(ContRazSoc,1,100) as RazaoSocial, " +
                    "substring(contas.ContaID +' - '+ ContFant,1,100) as Fantasia,substring(ContCGC,1,18) as cnpj,substring(ContIE,1,30) as IE, " +
                    "substring(Suframa,1,20) as suframatxt,substring(Logradouro +' , '+ numero,1,100) as rua,substring(cep,1,9) as ceptxt, " +
                    "substring(Bairro,1,30) as bairrotxt,substring(Municipios.Nome,1,50) as Cidade , substring(Municipios.Estado,1,2) as estadotxt, " +
                    "substring(ContInternetEMail,1,75) as Email, substring(ContTel1,1,30) as Telefone1, substring(ContTel2,1,30) as Telefone2 from contas  inner join enderecos on contas.entrega = enderecos.enderecoID  " +
                    "inner join Municipios on Enderecos.MunicipioID = Municipios.MunicipioID where ContCliente = 1 and ContEmpresa =0 and ContInativo=0 " +
                    "and ContPessoaFis=0 and Municipios.Estado not like'ex%'  and contas.contaID = '{0}'", Codigo),
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


                    var Email = Convert.ToString(RsCliente.Fields["Email"].Value.ToString());
                    //*****************Busca Email na String*********************
                    if (!string.IsNullOrEmpty(Email))
                    {
                        const string MatchEmailPattern =
                        @"(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
                        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
                        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
                        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})";
                        Regex rx = new Regex(MatchEmailPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        // Find matches. 
                        MatchCollection matches = rx.Matches(Email);
                        // Report the number of matches found. 
                        int noOfMatches = matches.Count;
                        // Report on each match. 
                        string MatchEmailList = "";
                        if (noOfMatches != 0)
                        {
                            foreach (Match match in matches)
                            {
                                MatchEmailList += match.Value.ToString();
                            }
                        }
                        if (!string.IsNullOrEmpty(MatchEmailList))
                        {
                            Retorno.emails = new List<Email>
                        {
                            new Email { email = MatchEmailList }
                        };
                        }
                    } 

                    string Telefone1 = digitsOnly.Replace(RsCliente.Fields["Telefone1"].Value.ToString(), "");
                    string Telefone2 = digitsOnly.Replace(RsCliente.Fields["Telefone2"].Value.ToString(), "");

                    if (!string.IsNullOrEmpty(Telefone1) && !string.IsNullOrEmpty(Telefone2))
                    {
                        Retorno.telefones = new List<Telefones>
                        {
                            new Telefones { numero = String.Format("{0:(##) ####-####}", double.Parse(Telefone1)) },
                            new Telefones { numero = String.Format("{0:(##) ####-####}", double.Parse(Telefone2)) }
                        };
                    }
                    else if (!string.IsNullOrEmpty(Telefone1) && string.IsNullOrEmpty(Telefone2))
                    {
                        Retorno.telefones = new List<Telefones>
                        {
                            new Telefones { numero = String.Format("{0:(##) ####-####}", double.Parse(Telefone1)) }
                        };
                    }
                    else if (string.IsNullOrEmpty(Telefone1) && !string.IsNullOrEmpty(Telefone2))
                    {
                        Retorno.telefones = new List<Telefones>
                        {
                            new Telefones { numero = String.Format("{0:(##) ####-####}", double.Parse(Telefone2)) }
                        };
                    }
                    Retorno.excluido = false;
                }
                try
                {
                    //Igonora campos nulos
                    string Json = JsonConvert.SerializeObject(Retorno, Formatting.Indented, new JsonSerializerSettings { });
                    var request = (HttpWebRequest)WebRequest.Create("http://sandbox.meuspedidos.com.br:8080/api/v1/clientes/");
                    var data = Encoding.ASCII.GetBytes(Json);
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
                    var ListarClientes = new PostClienteBO[RsCliente.RecordCount];
                    for (int i = 0; i < RsCliente.RecordCount; RsCliente.MoveNext(), i++)
                    {
                        ListarClientes[i] = GetCliente(RsCliente.Fields["ContaID"].Value.ToString(), cn);

                    }
                    RsCliente.Close();
                    return ListarClientes;
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