using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Domain;

namespace WebAPI.Identity.Dto
{
    public class VeiculoAgendamentoDto
    {
        public string categoria { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }
        public decimal ValorTotalLocacao { get; set; }
        public int TotalHorasLocacao { get; set; }
    }
}
