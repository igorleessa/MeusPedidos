﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPWebService.Skafe
{
    public class GetClienteWO
    {
        public int id { get; set; }
        public string razao_social { get; set; }
        public string nome_fantasia { get; set; }
        public string tipo { get; set; }
        public string cnpj { get; set; }
        public string Inscricao_estadual { get; set; }
        public string suframa { get; set; }
        public string rua { get; set; }
        public string complemento { get; set; }
        public string cep { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string estado { get; set; }
        public string observacao { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public bool excluido { get; set; }
    }
}