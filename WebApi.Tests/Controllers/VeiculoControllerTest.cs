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
            //TODO:Teste usuário sem pemissão         
        }
        
        [Fact]
        public void Veiculo_MarcaModeloInvalido_Test()
        {
            //TODO:Teste gerar marca modelo não encontrado   
        }

        [Fact]
        public void Veiculo_MarcaModeloValido_Test()
        {
            //TODO:Teste criar marca modelo válido         
        }

        [Fact]
        public void Veiculo_VeiculoInvalido_Test()
        {
            //TODO:Teste gerar veiculo não encontrado   
        }

        [Fact]
        public void Veiculo_VeiculoValido_Test()
        {
            //TODO:Teste criar veiculo válido         
        }

        [Fact]
        public void Veiculo_VeiculoJaCadastrado_Test()
        {
            //TODO:Teste criar veiculo já cadastrado         
        }

        [Fact]
        public void Veiculo_CheckListDevolucaoValido_Test()
        {
            //TODO:Teste checklist devolução válido         
        }

        [Fact]
        public void Veiculo_CheckListDevolucaoVeiculoNaoEncontrado_Test()
        {
            //TODO:Teste checklist devolução com veiculo não cadastrado         
        }

        [Fact]
        public void Veiculo_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
