using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Domain
{
    public class PerfilUsuario : IdentityUserRole<int>
    {
        public Usuario Usuario { get; set; }
        public Perfil Perfil { get; set; }
    }
}
