using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Domain;
using WebAPI.Identity.Dto;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly RoleManager<Perfil> _roleManager;
        private readonly UserManager<Usuario> _userManager;

        public PerfilController(RoleManager<Perfil> roleManager, UserManager<Usuario> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: api/Perfil
        //Apenas exemplo para mostrar que de acordo com perfil se tem a autorização
        [HttpGet] 
        [Authorize(Roles = "Operador")]
        public IActionResult Get()
        {
            return Ok(new { 
               role = new PerfilDto(),
               updateUserRole = new AtualizaPerfilUsuarioDto()
            });
        }

        // GET: api/Perfil/5
        //Apenas exemplo para mostrar que de acordo com perfil se tem a autorização
        [HttpGet("{id}", Name = "Get")]
        [Authorize(Roles = "Cliente")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Role/CriaPerfil
        //Método utilizado para criar o perfil(Operador ou Cliente ou etc...)
        [HttpPost("CriaPerfil")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> CriaPerfil(PerfilDto perfilDto)
        {
            try
            {
                var retorno = await _roleManager.CreateAsync(new Perfil { Name = perfilDto.Nome });

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }

        //Método utilizado para atualizar o relacionamento entre usuário e perfil(Operador ou Cliente)
        [HttpPut("AtualizaPerfilUsuario")]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> AtualizaPerfilUsuario(AtualizaPerfilUsuarioDto model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Nome);

                if (user != null)
                {
                    if (model.Delete)
                        await _userManager.RemoveFromRoleAsync(user, model.Perfil);
                    else
                        await _userManager.AddToRoleAsync(user, model.Perfil);
                }
                else
                {
                    return Ok("Usuário não encontrado");
                }

                return Ok("Sucesso");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }
    }
}
