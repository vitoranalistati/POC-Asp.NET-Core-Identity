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
            //TODO:Teste usuário sem pemissão         
        }

        [Fact]
        public void Perfil_UsuarioNaoEncontrado_Test()
        {
            //TODO:Teste usuário não encontrado         
        }

        [Fact]
        public void Perfil_AtualizaNaoExiste_Test()
        {
            //TODO:Teste atualiza perfil não encontrado   
        }

        [Fact]
        public void Perfil_Atualiza_Test()
        {
            //TODO:Teste atualiza perfil válido         
        }

        [Fact]
        public void Perfil_Cria_Test()
        {
            //TODO:Teste gerar perfil válido         
        }
        
        [Fact]
        public void Perfil_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
