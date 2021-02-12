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
    public class CotacaoControllerTest
    {
        [Fact]
        public void Cotacao_UsuarioSemPermissao_Test()
        {
            //TODO:Teste usuário sem pemissão         
        }

        [Fact]
        public void Cotacao_UsuarioNaoEncontrado_Test()
        {
            //TODO:Teste usuário não encontrado         
        }

        [Fact]
        public void Cotacao_VeiculoNaoExiste_Test()
        {
            //TODO:Teste gerar cotação com veiculo não encontrado   
        }

        [Fact]
        public void Cotacao_Simular_Test()
        {
            //TODO:Teste gerar cotação válida
        }
        
        [Fact]
        public void Cotacao_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
