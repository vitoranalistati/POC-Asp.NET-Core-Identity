using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using WebAPI.Identity.Controllers;
using WebAPI.Identity.Dto;
using WebAPI.Repository;
using WebAPI.Tests.Fixtures;
using WebAPI.Tests.Mocks;
using Xunit;

namespace WebAPI.Tests.Controllers
{
    [Collection("Mapper")]
    public class ContratoControllerTest
    {
        [Fact]
        public void Contrato_UsuarioSemPermissao_Test()
        {
            //TODO:Teste usuário sem pemissão         
        }

        [Fact]
        public void Contrato_UsuarioNaoEncontrado_Test()
        {
            //TODO:Teste usuário não encontrado         
        }

        [Fact]
        public void Contrato_VeiculoNaoExiste_Test()
        {
            //TODO:Teste gerar contrato locação com veiculo não encontrado  
        }

        [Fact]
        public void Contrato_GerarContratoLocacao_Test()
        {
            //TODO:Teste Gerar Contrato Locação      
        }
        
        [Fact]
        public void Contrato_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
