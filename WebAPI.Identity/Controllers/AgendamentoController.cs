using System;
using System.Threading.Tasks;
using AutoMapper;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Identity.Dto;
using WebAPI.Repository;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {        
        private readonly Context _context;        
        private readonly IMapper _mapper;

        public AgendamentoController(IMapper mapper, Context context)
        {
            _context = context;
            _mapper = mapper;
        }
        
        // POST: apiAgendamento/SimularAgendamento  
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("SimularAgendamento")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> SimularAgendamento(VeiculoAgendamentoDto veiculoAgendamento)
        {
            try
            {
                if (veiculoAgendamento.DataInicial == DateTime.MinValue)
                    return BadRequest($"{nameof(veiculoAgendamento.DataInicial)} deve ser informado.");

                if (veiculoAgendamento.DataFinal == DateTime.MinValue)
                    return BadRequest($"{nameof(veiculoAgendamento.DataFinal)} deve ser informado.");

                if (string.IsNullOrEmpty(veiculoAgendamento.categoria))
                    return BadRequest($"{nameof(veiculoAgendamento.categoria)} deve ser informado.");

                var veiculoExiste = await _context.Veiculos.FirstOrDefaultAsync(x => x.Categoria.ToUpper().Trim() == veiculoAgendamento.categoria.ToUpper().Trim());

                if (veiculoExiste == null)
                {
                    return BadRequest($"{nameof(veiculoAgendamento.categoria)} não existe.");
                }
                else
                {                    
                    TimeSpan ts = veiculoAgendamento.DataFinal - veiculoAgendamento.DataInicial;

                    veiculoAgendamento.TotalHorasLocacao = Convert.ToInt32(Math.Ceiling(ts.TotalHours));
                    veiculoAgendamento.ValorTotalLocacao = Convert.ToDecimal(veiculoAgendamento.TotalHorasLocacao) * veiculoExiste.ValorHora;

                    return Ok(veiculoAgendamento);

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
