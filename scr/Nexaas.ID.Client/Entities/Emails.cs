using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class Emails
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("emails")]
        public IEnumerable<string> EmailAddresses { get; set; } = new List<string>();
    }
}