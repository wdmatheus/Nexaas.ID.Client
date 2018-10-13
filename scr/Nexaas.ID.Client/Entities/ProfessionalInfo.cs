using System;
using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class ProfessionalInfo
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("profession")]
        public string Profession { get; set; }

        [JsonProperty("company")]
        public string Company { get; set; }

        [JsonProperty("position")]
        public string Position { get; set; }
    }
}
