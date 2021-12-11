using ClientFlurl.Entities;
using System.Threading.Tasks;

namespace ClientFlurl.Services
{
    public interface IViaCepClient
    {
        Task<Address> GetAddressByZipCode(string cep);
    }
}
