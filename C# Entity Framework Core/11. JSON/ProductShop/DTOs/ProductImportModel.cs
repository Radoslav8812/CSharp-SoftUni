using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ProductShop.DTOs
{
    [JsonObject]
    public class ProductImportModel
    {
        public ProductImportModel()
        {
        }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [Required]
        [JsonProperty("SellerId")]
        public int? SellerId { get; set; }

        [Required]
        [JsonProperty("BuyerId")]
        public int? BuyerId { get; set; }
    }
}

