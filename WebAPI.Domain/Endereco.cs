using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace WebAPI.Domain
{
    public class Endereco
    {
        [Key]
        public int Id { get; set; }
        public int Cep { get; set; }
        public string Logradouro { get; set; }        
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }        
        public Usuario Usuario { get; set; }
        public int UsuarioId { get; set; }
    }
}
