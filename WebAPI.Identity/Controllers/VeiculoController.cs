using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Domain;
using WebAPI.Identity.Dto;
using WebAPI.Repository;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VeiculoController : ControllerBase
    {        
        private readonly Context _context;

        public VeiculoController(Context context)
        {
            _context = context;
        }

        // GET: api/Veiculo
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            return Ok(new Veiculo());
        }

        // POST: apiVeiculo/CriaMarcaModelo        
        [HttpPost("CriaMarcaModelo")]
        [AllowAnonymous]
        public async Task<IActionResult> CriaMarcaModelo(MarcaModeloVeiculo marcaModeloVeiculo)
        {
            try
            {
                if (string.IsNullOrEmpty(marcaModeloVeiculo.Marca))
                    return BadRequest($"{nameof(marcaModeloVeiculo.Marca)} deve ser informado.");

                if (string.IsNullOrEmpty(marcaModeloVeiculo.Modelo))
                    return BadRequest($"{nameof(marcaModeloVeiculo.Modelo)} deve ser informado.");

                _context.MarcaModeloVeiculos.Add(marcaModeloVeiculo);
                await _context.SaveChangesAsync();                

                return Ok(marcaModeloVeiculo);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }

        // POST: apiVeiculo/CriaVeiculo        
        [HttpPost("CriaVeiculo")]
        [AllowAnonymous]
        public async Task<IActionResult> CriaVeiculo(Veiculo veiculo)
        {
            try
            {
                if (veiculo.MarcaModeloVeiculo == null)
                    return BadRequest($"{nameof(veiculo.MarcaModeloVeiculo)} deve ser informado.");

                if (string.IsNullOrEmpty(veiculo.MarcaModeloVeiculo.Marca))
                    return BadRequest($"{nameof(veiculo.MarcaModeloVeiculo.Marca)} deve ser informado.");

                if (string.IsNullOrEmpty(veiculo.Placa))
                    return BadRequest($"{nameof(veiculo.Placa)} deve ser informado.");


                var marcaModelo = await _context.MarcaModeloVeiculos.FirstOrDefaultAsync(x => x.Marca.ToUpper().Trim() == veiculo.MarcaModeloVeiculo.Marca.ToUpper().Trim()
                                                                                         && x.Modelo.ToUpper().Trim() == veiculo.MarcaModeloVeiculo.Modelo.ToUpper().Trim());

                if(marcaModelo == null)
                {
                    _context.MarcaModeloVeiculos.Add(veiculo.MarcaModeloVeiculo);
                    await _context.SaveChangesAsync();
                }

                var veiculoExiste = await _context.Veiculos.FirstOrDefaultAsync(x => x.Placa.ToUpper().Trim() == veiculo.Placa.ToUpper().Trim());

                if (veiculoExiste == null)
                {
                    _context.Veiculos.Add(veiculo);
                    await _context.SaveChangesAsync();
                } 
                else
                {
                    return BadRequest($"{nameof(veiculo.Placa)} já cadastrada.");
                }

                return Ok(veiculo);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }


        // POST: apiVeiculo/SimularLocacao        
        [HttpPost("SimularLocacao")]
        [AllowAnonymous]
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

        // POST: apiVeiculo/SimularAgendamento        
        [HttpPost("SimularAgendamento")]
        [AllowAnonymous]
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
