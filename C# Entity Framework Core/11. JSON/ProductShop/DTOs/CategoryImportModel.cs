using System;
using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    [JsonObject]
    public class CategoryImportModel
    {
        public CategoryImportModel()
        {
        }

        [JsonProperty("name")]
        public string Name { get; set; }
        
    }
}

