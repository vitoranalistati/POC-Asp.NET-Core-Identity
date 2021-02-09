using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Identity.Dto
{
    public class AtualizaPerfilUsuarioDto
    {
        public string Nome { get; set; }
        public string Perfil { get; set; }
        public bool Delete { get; set; }
    }
}
