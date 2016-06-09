using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPWebService.Skafe
{
    public class PutPrecoBO
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string tipo { get; set; }
        public float acrescimo { get; set; }
        public float desconto { get; set; }
        public bool excluido { get; set; }
    }
}