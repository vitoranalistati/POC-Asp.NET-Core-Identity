using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using WebAPI.Domain;

namespace WebAPI.Repository
{
    public class Context : IdentityDbContext<Usuario, Perfil, int,
                                             IdentityUserClaim<int>, PerfilUsuario, IdentityUserLogin<int>,
                                             IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public Context(DbContextOptions<Context> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PerfilUsuario>(perfilUsuario => 
            {
                perfilUsuario.HasKey(ur => new { ur.UserId, ur.RoleId });

                perfilUsuario.HasOne(ur => ur.Perfil)
                        .WithMany(r => r.PerfisUsuario)
                        .HasForeignKey(ur => ur.RoleId)
                        .IsRequired();

                perfilUsuario.HasOne(ur => ur.Usuario)
                        .WithMany(r => r.PerfisUsuario)
                        .HasForeignKey(ur => ur.UserId)
                        .IsRequired();
            });            
        }

        public DbSet<MarcaModeloVeiculo> MarcaModeloVeiculos { get; set; }
        public DbSet<Veiculo> Veiculos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
    }
}
