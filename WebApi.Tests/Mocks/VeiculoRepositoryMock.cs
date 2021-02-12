using System.Threading.Tasks;
using WebAPI.Identity.Dto;

namespace WebAPI.Tests.Mocks
{
    public class VeiculoRepositoryMock     {
        public Task SimularAgendamento()
        {
            return Task.FromResult("Ok");
        }
    }
}
