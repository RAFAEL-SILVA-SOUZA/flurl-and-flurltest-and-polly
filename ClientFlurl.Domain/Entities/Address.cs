using Newtonsoft.Json;

namespace ClientFlurl.Entities
{

    public class Address
    {

        [JsonProperty("cep")]
        public string ZipCode { get; set; }
        [JsonProperty("logradouro")]
        public string PublicPlace { get; set; }
        [JsonProperty("complemento")]
        public string Complement { get; set; }
        [JsonProperty("bairro")]
        public string District { get; set; }
        [JsonProperty("localidade")]
        public string Location { get; set; }
        [JsonProperty("uf")]
        public string FederativeUnit { get; set; }
        [JsonProperty("ibge")]
        public string Ibge { get; set; }
        [JsonProperty("gia")]
        public string Gia { get; set; }
        [JsonProperty("ddd")]
        public string Ddd { get; set; }
        [JsonProperty("siafi")]
        public string Siafi { get; set; }

        public bool IsNotValid()
        {
            return string.IsNullOrWhiteSpace(ZipCode);
        }
    }

}
