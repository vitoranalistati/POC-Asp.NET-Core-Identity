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
    public class PerfilControllerTest
    {
        [Fact]
        public void Perfil_UsuarioSemPermissao_Test()
        {
            //TODO:Teste usu�rio sem pemiss�o         
        }

        [Fact]
        public void Perfil_UsuarioNaoEncontrado_Test()
        {
            //TODO:Teste usu�rio n�o encontrado         
        }

        [Fact]
        public void Perfil_AtualizaNaoExiste_Test()
        {
            //TODO:Teste atualiza perfil n�o encontrado   
        }

        [Fact]
        public void Perfil_Atualiza_Test()
        {
            //TODO:Teste atualiza perfil v�lido         
        }

        [Fact]
        public void Perfil_Cria_Test()
        {
            //TODO:Teste gerar perfil v�lido         
        }
        
        [Fact]
        public void Perfil_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
