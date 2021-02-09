using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Domain
{
    public class MarcaModeloVeiculo 
    {
        [Key]
        public int Id { get; set; }
        public string Marca { get; set; }        
        public string Modelo { get; set; }               
    }
}
