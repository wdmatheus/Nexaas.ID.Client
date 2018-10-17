using System;
using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class ApplicationInvitiationResponse
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("requester")]
        public Guid? Requester { get; set; }
    }
}
