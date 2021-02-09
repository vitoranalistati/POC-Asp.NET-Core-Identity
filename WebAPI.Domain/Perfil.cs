using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Domain
{
    public class Perfil : IdentityRole<int>
    {
        public List<PerfilUsuario> PerfisUsuario { get; set; }
    }
}
