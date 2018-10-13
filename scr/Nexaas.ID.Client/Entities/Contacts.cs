using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class Contacts
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("phone_numbers")]
        public IEnumerable<string> PhoneNumbers { get; set; } = new List<string>();
    }
}