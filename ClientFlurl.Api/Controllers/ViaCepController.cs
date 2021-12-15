using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ClientFlurl.Domain.Services.Contracts;

namespace ClientFlurl.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ViaCepController : ControllerBase
    {
        private readonly IViaCepClient viaCepClient;

        public ViaCepController(IViaCepClient viaCepClient)
        => this.viaCepClient = viaCepClient;

        [HttpGet]
        public async Task<IActionResult> Get(string zipCode = "24740500")
        {
            var endereco = await viaCepClient.GetAddressByZipCode(zipCode);
            return endereco != default ? Ok(endereco) : NoContent();
        }
    }
}
