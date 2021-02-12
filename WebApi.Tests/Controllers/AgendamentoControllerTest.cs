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
    public class AgendamentoControllerTest
    {
        [Fact]
        public void Agendamento_UsuarioSemPermissao_Test()
        {
            //TODO:Teste usu�rio sem pemiss�o         
        }

        [Fact]
        public void Agendamento_UsuarioNaoEncontrado_Test()
        {
            //TODO:Teste usu�rio n�o encontrado         
        }

        [Fact]
        public void Agendamento_VeiculoNaoExiste_Test()
        {
            //TODO:Teste gerar agendamento com veiculo n�o encontrado   
        }

        [Fact]
        public void Agendamento_Simular_Test()
        {
            //TODO:Teste gerar agendamento v�lido         
        }
        
        [Fact]
        public void Agendamento_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
