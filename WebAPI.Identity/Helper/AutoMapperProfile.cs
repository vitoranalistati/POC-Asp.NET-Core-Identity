using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Domain;
using WebAPI.Identity.Dto;

namespace WebAPI.Identity.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioDto>().ReverseMap();
            CreateMap<Usuario, UsuarioLoginDto>().ReverseMap();
        }
    }
}
