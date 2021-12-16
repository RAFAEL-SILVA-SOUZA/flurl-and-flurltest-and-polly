using ClientFlurl.Entities;
using System.Threading.Tasks;

namespace ClientFlurl.Tests.Unit
{
    public static class Mocks
    {
        public static Address AddressCorrect()
        {
            return new Address
            {
                ZipCode = "24740-500",
                PublicPlace = "Rua Libanio Ratazi",
                Complement = "",
                District = "Coelho",
                Location = "São Gonçalo",
                FederativeUnit = "RJ",
                Ibge = "3304904",
                Gia = "",
                Ddd = "21",
                Siafi = "5897"
            };
        }

        public static Address AddressIncorrect()
        {
            return new Address
            {
                ZipCode = "24740-500",
                PublicPlace = "Rua Libanio",
                Complement = "",
                District = "Coelho",
                Location = "São Gonçalo",
                FederativeUnit = "R",
                Ibge = "3304904",
                Gia = "",
                Ddd = "21",
                Siafi = "5897"
            };
        }
    }

}
