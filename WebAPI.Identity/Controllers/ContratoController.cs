using System;
using System.Threading.Tasks;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Identity.Dto;
using WebAPI.Repository;
using DinkToPdf;
using WebAPI.Identity.Utility;
using System.IO;

namespace WebAPI.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContratoController : ControllerBase
    {        
        private readonly Context _context;
        private readonly IConverter _converter;

        public ContratoController(Context context, IConverter converter)
        {
            _context = context;
            _converter = converter;
        }
        
        [HttpPost("GerarContratoLocacao")]        
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize(Roles = "Operador")]
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
    }
}
