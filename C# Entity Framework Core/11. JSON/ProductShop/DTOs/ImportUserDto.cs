using System;
using Newtonsoft.Json;

namespace ProductShop.DTOs.User
{
    [JsonObject]
    public class ImportUserDto
    {
        public ImportUserDto()
        {
        }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int? Age { get; set; }
    }
}

