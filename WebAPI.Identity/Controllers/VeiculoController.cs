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
    public class VeiculoController : ControllerBase
    {        
        private readonly Context _context;
        private IConverter _converter;

        public VeiculoController(Context context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }

        // GET: api/Veiculo
        /// <summary>
        /// Obtém o modelo Json Veiculo. 
        /// </summary>
        /// 
        /// <returns>Modelo Veiculo</returns>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult Get()
        {
            return Ok(new Veiculo());
        }

        // POST: apiVeiculo/CriaMarcaModelo  
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("CriaMarcaModelo")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
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
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
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
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("SimularLocacao")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
                    //TODO: Refatorar
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
        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("SimularAgendamento")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
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
                    //TODO: Refatorar
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

        // POST: apiVeiculo/CheckListDevolucao        

        /// <response code="201">Notificações enviadas</response>
        /// <response code="400">Parâmetros inválidos</response>
        /// <response code="401">Sem autorização</response>
        /// <response code="500">Erro interno</response>
        [HttpPost("CheckListDevolucao")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]        
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
                    //TODO: Refatorar
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


        [HttpPost("GerarContratoLocacao")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GerarContratoLocacao(VeiculoSimulacaoDto veiculoSimulacao)
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
                    //TODO: Refatorar
                    TimeSpan ts = veiculoSimulacao.DataFinal - veiculoSimulacao.DataInicial;

                    veiculoSimulacao.TotalHorasLocacao = Convert.ToInt32(Math.Ceiling(ts.TotalHours));
                    veiculoSimulacao.ValorTotalLocacao = Convert.ToDecimal(veiculoSimulacao.TotalHorasLocacao) * veiculoExiste.ValorHora;

                    var globalSettings = new GlobalSettings
                    {
                        ColorMode = ColorMode.Color,
                        Orientation = Orientation.Portrait,
                        PaperSize = PaperKind.A4,
                        Margins = new MarginSettings { Top = 10 },
                        DocumentTitle = "Contrato de Locação"
                    };
                    var objectSettings = new ObjectSettings
                    {
                        PagesCount = true,
                        HtmlContent = TemplateGenerator.GetHTMLString(veiculoSimulacao),
                        WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                        HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                        FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Fim" }
                    };
                    var pdf = new HtmlToPdfDocument()
                    {
                        GlobalSettings = globalSettings,
                        Objects = { objectSettings }
                    };

                    var file = _converter.Convert(pdf);
                    return File(file, "application/pdf", "ContratoLocacao.pdf");

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
