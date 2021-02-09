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

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IMapper _mapper;

        public UsuarioController(IConfiguration config, UserManager<Usuario> userManager,
                              SignInManager<Usuario> signInManager, IMapper mapper)
        {
            _config = config;
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        // GET: api/Usuario
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new UsuarioDto());
        }

        // GET: api/Usuario/5
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UsuarioLoginDto usuarioLogin)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioLogin.Nome))
                    return BadRequest($"{nameof(usuarioLogin.Nome)} deve ser informado.");

                if (usuarioLogin.Nome.Length < 6 || usuarioLogin.Nome.Length > 11)
                    return BadRequest($"{nameof(usuarioLogin.Nome)} deve ter no mínimo 6 e máximo 11 dígitos. Nome: {usuarioLogin.Nome}");

                if (!usuarioLogin.Nome.All(char.IsDigit))
                    return BadRequest($"{nameof(usuarioLogin.Nome)} deve ser número.");
                                
                var user = await _userManager.FindByNameAsync(usuarioLogin.Nome);

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

        // POST: api/Registrar
        [HttpPost("Registrar")]
        [AllowAnonymous]
        public async Task<IActionResult> Registrar(UsuarioDto usuarioDto)
        {
            try
            {
                if (string.IsNullOrEmpty(usuarioDto.Login))
                    return BadRequest($"{nameof(usuarioDto.Login)} deve ser informado.");

                if (usuarioDto.Login.Length < 6 || usuarioDto.Login.Length > 11)
                    return BadRequest($"{nameof(usuarioDto.Nome)} deve ter no mínimo 6 e máximo 11 dígitos. Nome: {usuarioDto.Login}");

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
                        DataNascimento = usuarioDto.DataNascimento,
                        
                    };

                    var result = await _userManager.CreateAsync(usuario, usuarioDto.Password);

                    if (result.Succeeded)
                    {
                        var appUsuario = await _userManager.Users
                            .FirstOrDefaultAsync(u => u.NormalizedUserName == usuario.UserName.ToUpper());

                        var token = GenerateJWToken(appUsuario).Result;

                        var user = await _userManager.FindByNameAsync(usuario.UserName);

                        if (user != null)
                        {
                            //TODO refatorar para tirar acoplamento.
                            if(usuario.UserName.Length == 11)
                                await _userManager.AddToRoleAsync(user, "Cliente");
                            else
                                await _userManager.AddToRoleAsync(user, "Operador");
                        }                        

                        return Ok(token);
                    }
                }

                return Unauthorized();
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
