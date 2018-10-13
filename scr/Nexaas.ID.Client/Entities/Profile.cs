using System;
using Newtonsoft.Json;

namespace Nexaas.ID.Client.Entities
{
    public class Profile
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("birth")]
        public DateTime? Birth { get; set; }

        [JsonProperty("gender")]
        public string Gender { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("picture")]
        public string Picture { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        public ProfessionalInfo ProfessionalInfo { get; set; }

        public Contacts Contacts { get; set; }

        public Emails Emails { get; set; }
    }
}

