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
using WebAPI.Repository;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnderecoController : ControllerBase
    {        
        private readonly Context _context;

        public EnderecoController(Context context)
        {
            _context = context;
        }

        // GET: api/Endereco
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new Endereco());
        }

        // POST: apiEndereco/CriaEndereco        
        [HttpPost("CriaEndereco")]
        [AllowAnonymous]
        public async Task<IActionResult> CriaEndereco(Endereco endereco)
        {
            try
            {
                _context.Enderecos.Add(endereco);
                await _context.SaveChangesAsync();                

                return Ok(endereco);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }        
    }
}
