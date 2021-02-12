using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
                
        // POST: apiVeiculo/CriaMarcaModelo  
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("CriaMarcaModelo")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador")]
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
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("CriaVeiculo")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador")]
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

        // POST: apiVeiculo/CheckListDevolucao
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("CheckListDevolucao")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador")]
        public async Task<IActionResult> CheckListDevolucao(CheckListDevolucaoDto checkListDevolucao)
        {
            try
            {
                if (checkListDevolucao.DataInicial == DateTime.MinValue)
                    return BadRequest($"{nameof(checkListDevolucao.DataInicial)} deve ser informado.");

                if (checkListDevolucao.DataFinal == DateTime.MinValue)
                    return BadRequest($"{nameof(checkListDevolucao.DataFinal)} deve ser informado.");

                if (string.IsNullOrEmpty(checkListDevolucao.Placa))
                    return BadRequest($"{nameof(checkListDevolucao.Placa)} deve ser informado.");

                var veiculoExiste = await _context.Veiculos.FirstOrDefaultAsync(x => x.Placa.ToUpper().Trim() == checkListDevolucao.Placa.ToUpper().Trim());

                if (veiculoExiste == null)
                {
                    return BadRequest($"{nameof(checkListDevolucao.Placa)} não existe.");
                }
                else
                {                    
                    TimeSpan ts = checkListDevolucao.DataFinal - checkListDevolucao.DataInicial;

                    checkListDevolucao.TotalHorasLocacao = Convert.ToInt32(Math.Ceiling(ts.TotalHours));
                    checkListDevolucao.ValorTotalLocacao = Convert.ToDecimal(checkListDevolucao.TotalHorasLocacao) * veiculoExiste.ValorHora;
                    checkListDevolucao.ValorHora = veiculoExiste.ValorHora;

                    CalcularCustoAdicional(checkListDevolucao);

                    return Ok(checkListDevolucao);

                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"ERROR {ex.Message}");
            }
        }
        
        private static void CalcularCustoAdicional(CheckListDevolucaoDto checkListDevolucao)
        {
            decimal custoTotalItem = (checkListDevolucao.PercentualCustoAdicional / 100) * checkListDevolucao.ValorTotalLocacao;  
            
            if (!checkListDevolucao.CarroLimpo)
            {            
                checkListDevolucao.ValorTotalLocacao = checkListDevolucao.ValorTotalLocacao + custoTotalItem;
                checkListDevolucao.CustoTotalComItensDevolucao += custoTotalItem;
            }
            if (!checkListDevolucao.TanqueCheio)
            {
                checkListDevolucao.ValorTotalLocacao = checkListDevolucao.ValorTotalLocacao + custoTotalItem;
                checkListDevolucao.CustoTotalComItensDevolucao += custoTotalItem;
            }
            if (!checkListDevolucao.Amassados)
            {
                checkListDevolucao.ValorTotalLocacao = checkListDevolucao.ValorTotalLocacao + custoTotalItem;
                checkListDevolucao.CustoTotalComItensDevolucao += custoTotalItem;
            }
            if (!checkListDevolucao.Arranhoes)
            {
                checkListDevolucao.ValorTotalLocacao = checkListDevolucao.ValorTotalLocacao + custoTotalItem;
                checkListDevolucao.CustoTotalComItensDevolucao += custoTotalItem;
            }
        }
    }
}
