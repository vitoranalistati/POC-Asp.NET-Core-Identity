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
    public class UsuarioControllerTest
    {        
        [Fact]
        public void Usuario_LoginNaoEncontrado_Test()
        {
            //TODO:Teste login não encontrado         
        }

        [Fact]
        public void Usuario_Login_Test()
        {
            //TODO:Teste login válido         
        }

        [Fact]
        public void Usuario_LoginSenhaInvalida_Test()
        {
            //TODO:Teste login senha incorreta         
        }

        [Fact]
        public void Usuario_RegistrarJaExiste_Test()
        {
            //TODO:Teste registrar usuario que já existe   
        }

        [Fact]
        public void Usuario_Registrar_Test()
        {
            //TODO:Teste registrar válido         
        }
        
        [Fact]
        public void Usuario_InternalErroServer_Test()
        {
            //TODO:Teste erro 500         
        }
    }
}
