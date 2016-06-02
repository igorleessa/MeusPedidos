using ADODB;
using MPWebService.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPWebService.Skafe
{
    public class PostPrecoFO
    {
        public static PostPrecoBO[] ListarPostPreco(string JsonChamada, PostPrecoWO Chamada, Connection cn)
        {
            var Serializer = new SerializerFO();
            try
            {
                if (!String.IsNullOrEmpty(JsonChamada))
                    Chamada = (PostPrecoWO)Serializer.DeserializarObjetoJson(JsonChamada, Chamada);

                var Preco = PostPrecoDA.ListPreco(cn);

                return Preco;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}