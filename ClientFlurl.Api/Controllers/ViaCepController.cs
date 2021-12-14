using Microsoft.AspNetCore.Mvc;
using ClientFlurl.Services;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Get(string zipCode)
        {
            var endereco = await viaCepClient.GetAddressByZipCode(zipCode);
            return endereco != default ? Ok(endereco) : NoContent();
        }
    }
}
