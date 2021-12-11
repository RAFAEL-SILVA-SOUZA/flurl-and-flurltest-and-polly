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
        {
            this.viaCepClient = viaCepClient;
        }

        [HttpGet]
        public async Task<IActionResult> Get(string cep)
        {
            var endereco = await viaCepClient.GetAddressByZipCode(cep);

            if (endereco == default) return NoContent();

            return Ok(endereco);
        }
    }
}
