using ClientFlurl.Entities;
using System.Threading.Tasks;

namespace ClientFlurl.Domain.Services.Contracts
{
    public interface IViaCepClient
    {
        Task<Address> GetAddressByZipCode(string zipCode);
    }
}
