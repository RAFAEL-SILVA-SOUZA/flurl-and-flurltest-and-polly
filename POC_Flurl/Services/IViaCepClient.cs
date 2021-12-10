using POC_Flurl.Entities;
using System.Threading.Tasks;

namespace POC_Flurl.Services
{
    public interface IViaCepClient
    {
        Task<Address> GetAddressByZipCode(string cep);
    }
}
