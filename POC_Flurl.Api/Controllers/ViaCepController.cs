using Microsoft.AspNetCore.Mvc;
using POC_Flurl.Services;
using System.Threading.Tasks;

namespace POC_Flurl.Api.Controllers
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
