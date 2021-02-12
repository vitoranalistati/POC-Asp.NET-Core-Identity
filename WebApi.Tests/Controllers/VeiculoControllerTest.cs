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
    public class VeiculoControllerTest
    {
        [Fact]
        public void Veiculo_UsuarioSemPermissao_Test()
        {
            //TODO:Teste usu�rio sem pemiss�o         
        }
        
        [Fact]
        public void Veiculo_MarcaModeloInvalido_Test()
        {
            //TODO:Teste gerar marca modelo n�o encontrado   
        }

        [Fact]
        public void Veiculo_MarcaModeloValido_Test()
        {
            //TODO:Teste criar marca modelo v�lido         
        }

        [Fact]
        public void Veiculo_VeiculoInvalido_Test()
        {
            //TODO:Teste gerar veiculo n�o encontrado   
        }

        [Fact]
        public void Veiculo_VeiculoValido_Test()
        {
            //TODO:Teste criar veiculo v�lido         
        }

        [Fact]
        public void Veiculo_VeiculoJaCadastrado_Test()
        {
            //TODO:Teste criar veiculo j� cadastrado         
        }

        [Fact]
        public void Veiculo_CheckListDevolucaoValido_Test()
        {
            //TODO:Teste checklist devolu��o v�lido         
        }

        [Fact]
        public void Veiculo_CheckListDevolucaoVeiculoNaoEncontrado_Test()
        {
            //TODO:Teste checklist devolu��o com veiculo n�o cadastrado         
        }

        [Fact]
        public void Veiculo_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
