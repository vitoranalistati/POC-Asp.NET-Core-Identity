using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Domain;
using WebAPI.Identity.Dto;
using WebAPI.Repository;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly RoleManager<Perfil> _roleManager;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public UsuarioController(IConfiguration config, UserManager<Usuario> userManager, RoleManager<Perfil> roleManager,
                              SignInManager<Usuario> signInManager, IMapper mapper, Context context)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _context = context;
        }
                
        // GET: apiUsuario/Login
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("Login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioLogin.Login))
                    return BadRequest($"{nameof(usuarioLogin.Login)} deve ser informado.");

                if (usuarioLogin.Login.Length < 6 || usuarioLogin.Login.Length > 11)
                    return BadRequest($"{nameof(usuarioLogin.Login)} deve ter no mínimo 6 e máximo 11 dígitos. Nome: {usuarioLogin.Login}");

                if (!usuarioLogin.Login.All(char.IsDigit))
                    return BadRequest($"{nameof(usuarioLogin.Login)} deve ser número.");
                                
                var user = await _userManager.FindByNameAsync(usuarioLogin.Login);

                var result = await _signInManager
                    .CheckPasswordSignInAsync(user, usuarioLogin.Password, false);

                if(result.Succeeded)
                {
                    var appUser = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == user.UserName.ToUpper());

                    var userToReturn = _mapper.Map<UsuarioDto>(appUser);

                    return Ok(new
                    {
                        token = GenerateJWToken(appUser).Result,
                        user = userToReturn
                    });
                }

                return Unauthorized();
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }

        // POST: apiUsuario/Registrar
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("Registrar")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
        public async Task<IActionResult> Registrar(UsuarioDto usuarioDto)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioDto.Login))
                    return BadRequest($"{nameof(usuarioDto.Login)} deve ser informado.");

                if (usuarioDto.Login.Length < 6 || usuarioDto.Login.Length > 11)
                    return BadRequest($"{nameof(usuarioDto.Login)} deve ter no mínimo 6 e máximo 11 dígitos. Nome: {usuarioDto.Login}");

                if (!usuarioDto.Login.All(char.IsDigit))
                    return BadRequest($"{nameof(usuarioDto.Login)} deve ser número.");

                var usuario = await _userManager.FindByNameAsync(usuarioDto.Login);

                if (usuario == null)
                {
                    usuario = new Usuario
                    {
                        UserName = usuarioDto.Login,
                        Nome = usuarioDto.Nome,
                        Cpf = usuarioDto.Login.Length == 11 ? usuarioDto.Login : "",
                        Matricula = usuarioDto.Login.Length == 11 ? "" : usuarioDto.Login,
                        DataNascimento = usuarioDto.DataNascimento                        
                    };

                    var result = await _userManager.CreateAsync(usuario, usuarioDto.Password);

                    if (result.Succeeded)
                    {
                        var appUsuario = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == usuario.UserName.ToUpper());

                        var token = GenerateJWToken(appUsuario).Result;

                        //TODO refatorar para tirar acoplamento.
                        if (usuario.UserName.Length == 11)
                        {
                            var perfilCliente = await _roleManager.FindByNameAsync("Cliente");

                            if (perfilCliente == null)
                                await _roleManager.CreateAsync(new Perfil { Name = "Cliente" });

                            await _userManager.AddToRoleAsync(usuario, "Cliente");
                        }
                        else
                        {
                            var perfilCliente = await _roleManager.FindByNameAsync("Operador");

                            if (perfilCliente == null)
                                await _roleManager.CreateAsync(new Perfil { Name = "Operador" });

                            await _userManager.AddToRoleAsync(usuario, "Operador");
                        }

                        var endereco = new Endereco
                        {
                            Cep = usuarioDto.Endereco.Cep,
                            Cidade = usuarioDto.Endereco.Cidade,
                            Complemento = usuarioDto.Endereco.Complemento,
                            Logradouro = usuarioDto.Endereco.Logradouro,
                            Numero = usuarioDto.Endereco.Numero,
                            Estado = usuarioDto.Endereco.Estado,
                            UsuarioId = usuario.Id
                        };

                        _context.Enderecos.Add(endereco);
                        await _context.SaveChangesAsync();


                        return Ok(token);
                    }
                }

                return Ok("Usuário já existe");
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }

        private async Task<string> GenerateJWToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.UserName)
            };

            var roles = await _userManager.GetRolesAsync(usuario);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(
                _config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }        
    }
}
