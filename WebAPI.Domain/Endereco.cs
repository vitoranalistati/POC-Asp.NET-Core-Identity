using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Domain
{
    public class Endereco
    {
        public string Id { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }        
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
