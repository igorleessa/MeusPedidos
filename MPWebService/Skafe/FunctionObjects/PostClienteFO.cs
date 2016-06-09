using ADODB;
using MPWebService.CORE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MPWebService.Skafe
{
    public class PostClienteFO
    {
        public static PostClienteBO[] ListarPostCliente(string JsonChamada, PostClienteWO Chamada, Connection cn)
        {
            var Serializer = new SerializerFO();
            try
            {
                if (!String.IsNullOrEmpty(JsonChamada))
                    Chamada = (PostClienteWO)Serializer.DeserializarObjetoJson(JsonChamada, Chamada);

                var cliente = PostClienteDA.ListCliente(cn);

                return cliente;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}