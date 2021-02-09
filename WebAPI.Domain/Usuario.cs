using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace WebAPI.Domain
{
    public class Usuario : IdentityUser<int>
    {
        public string Nome { get; set; }        
        public string EnderecoId { get; set; }
        public string Cpf { get; set; }
        public string Matricula { get; set; }
        public DateTime DataNascimento { get; set; }
        public List<PerfilUsuario> PerfisUsuario { get; set; }
    }
}
