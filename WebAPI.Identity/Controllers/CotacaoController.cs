using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;
using WebAPI.Identity.Dto;
using WebAPI.Repository;
using DinkToPdf;
using WebAPI.Identity.Utility;
using System.IO;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CotacaoController : ControllerBase
    {        
        private readonly Context _context;
        
        public CotacaoController(Context context)
        {
            _context = context;            
        }

        // POST: apiCotacao/SimularLocacao  
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("SimularLocacao")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador, Cliente")]
        public async Task<IActionResult> SimularLocacao(VeiculoSimulacaoDto veiculoSimulacao)
        {
            try
            {
                if (veiculoSimulacao.DataInicial == DateTime.MinValue)
                    return BadRequest($"{nameof(veiculoSimulacao.DataInicial)} deve ser informado.");

                if (veiculoSimulacao.DataFinal == DateTime.MinValue)
                    return BadRequest($"{nameof(veiculoSimulacao.DataFinal)} deve ser informado.");

                if (string.IsNullOrEmpty(veiculoSimulacao.Placa))
                    return BadRequest($"{nameof(veiculoSimulacao.Placa)} deve ser informado.");

                var veiculoExiste = await _context.Veiculos.FirstOrDefaultAsync(x => x.Placa.ToUpper().Trim() == veiculoSimulacao.Placa.ToUpper().Trim());

                if (veiculoExiste == null)
                {
                    return BadRequest($"{nameof(veiculoSimulacao.Placa)} não existe.");
                }
                else
                {                    
                    TimeSpan ts = veiculoSimulacao.DataFinal - veiculoSimulacao.DataInicial;
                    
                    veiculoSimulacao.TotalHorasLocacao = Convert.ToInt32(Math.Ceiling(ts.TotalHours));
                    veiculoSimulacao.ValorTotalLocacao = Convert.ToDecimal(veiculoSimulacao.TotalHorasLocacao) * veiculoExiste.ValorHora;

                    return Ok(veiculoSimulacao);                    
                }                
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }        
    }
}
