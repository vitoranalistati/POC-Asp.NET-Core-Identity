using AutoMapper;
using WebAPI.Identity.Helper;

namespace WebAPI.Tests.Fixtures
{
    public class MapperFixture
    {
        public IMapper Mapper { get; }

        public MapperFixture()
        {
            var config = new MapperConfiguration(opts =>
            {
                opts.AddProfile(new AutoMapperProfile());                
            });

            Mapper = config.CreateMapper();
        }
    }
}
