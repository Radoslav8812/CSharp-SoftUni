using System;
using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    [JsonObject]
    public class CategoryProductImportModel
    {
        public CategoryProductImportModel()
        {
        }

        [JsonProperty("CategoryId")]
        public int CategoryId { get; set; }

        [JsonProperty("ProductId")]
        public int ProductId { get; set; }

    }
}

