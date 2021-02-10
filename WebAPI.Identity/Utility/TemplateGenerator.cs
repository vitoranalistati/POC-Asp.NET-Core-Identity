using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Identity.Dto;

namespace WebAPI.Identity.Utility
{    public static class TemplateGenerator
    {
        public static string GetHTMLString(VeiculoSimulacaoDto veiculo)
        {
            //TODO: Apenas exemplo de geração do pdf, layout pode validar o modelo.            
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>Contrato de Locação!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Placa</th>
                                        <th>Data Inicial</th>
                                        <th>Data Final</th>
                                        <th>Total Horas Locacao</th>
                                        <th>Valor Total Locacao</th>
                                    </tr>");           
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                    <td>{4}</td>
                                  </tr>", veiculo.Placa, veiculo.DataInicial, veiculo.DataFinal, veiculo.TotalHorasLocacao, veiculo.ValorTotalLocacao);
            
            sb.Append(@"
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
