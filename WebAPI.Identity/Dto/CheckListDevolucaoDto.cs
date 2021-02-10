using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Domain;

namespace WebAPI.Identity.Dto
{
    public class CheckListDevolucaoDto
    {
        public string Placa { get; set; }
        public DateTime DataInicial { get; set; }
        public DateTime DataFinal { get; set; }                
        public bool CarroLimpo { get; set; }
        public bool TanqueCheio { get; set; }
        public bool Amassados { get; set; }
        public bool Arranhoes { get; set; }
        public decimal PercentualCustoAdicional { get; set; }
        public decimal ValorHora { get; set; }
        public int TotalHorasLocacao { get; set; }
        public decimal ValorTotalLocacao { get; set; }
        public decimal CustoTotalComItensDevolucao { get; set; }
    }
}
