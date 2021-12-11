using Newtonsoft.Json;

namespace ClientFlurl.Entities
{

    public class Address
    {
        public Address(string ZipCode, string PublicPlace, 
                       string Complement, string Neighborhood, 
                       string Location, string uf, 
                       string ibge, string gia, 
                       string ddd, string siafi)
        {
            this.ZipCode = ZipCode;
            this.PublicPlace = PublicPlace;
            this.Complement = Complement;
            this.Neighborhood = Neighborhood;
            this.Location = Location;
            this.Uf = uf;
            this.Ibge = ibge;
            this.Gia = gia;
            this.Ddd = ddd;
            this.Siafi = siafi;
        }

        [JsonProperty("cep")]
        public string ZipCode { get; private set; }
        [JsonProperty("logradouro")]
        public string PublicPlace { get; private set; }

        [JsonProperty("complemento")]
        public string Complement { get; private set; }
        [JsonProperty("bairro")]
        public string Neighborhood { get; private set; }
        [JsonProperty("localidade")]
        public string Location { get; private set; }
        [JsonProperty("uf")]
        public string Uf { get; private set; }
        [JsonProperty("ibge")]
        public string Ibge { get; private set; }
        [JsonProperty("gia")]
        public string Gia { get; private set; }
        [JsonProperty("ddd")]
        public string Ddd { get; private set; }
        [JsonProperty("siafi")]
        public string Siafi { get; private set; }
    }

}
