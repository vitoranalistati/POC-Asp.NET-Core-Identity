using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Domain
{
    public class Veiculo
    {
        [Key]
        public int Id { get; set; }
        public string Placa { get; set; }        
        public int Ano { get; set; }
        public string Combustivel { get; set; }
        public decimal ValorHora { get; set; }
        public int LimitePortaMalas { get; set; }
        public string Categoria { get; set; }
        public MarcaModeloVeiculo MarcaModeloVeiculo { get; set; }
        public int MarcaModeloVeiculoId { get; set; }
    }
}
