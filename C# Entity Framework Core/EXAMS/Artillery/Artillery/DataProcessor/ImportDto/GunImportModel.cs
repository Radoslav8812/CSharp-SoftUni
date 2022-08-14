using System;
using System.ComponentModel.DataAnnotations;
using Artillery.Data.Models.Enums;
using Newtonsoft.Json;

namespace Artillery.DataProcessor.ImportDto
{
	[JsonObject]
	public class GunImportModel
	{
		[Required]
		[JsonProperty(nameof(ManufacturerId))]
		public int ManufacturerId { get; set; }

		[Required]
		[Range(100, 1350000)]
		[JsonProperty(nameof(GunWeight))]
		public int GunWeight { get; set; }

		[Required]
		[Range(2.00, 35.00)]
		[JsonProperty(nameof(BarrelLength))]
		public double BarrelLength { get; set; }

		[JsonProperty(nameof(NumberBuild))]
		public int? NumberBuild { get; set; }

		[Required]
		[Range(1, 100000)]
		[JsonProperty(nameof(Range))]
		public int Range { get; set; }

		[Required]
		[EnumDataType(typeof(GunType))]
        [JsonProperty(nameof(GunType))]
        public string GunType { get; set; }

		[Required]
        [JsonProperty(nameof(ShellId))]
        public int ShellId { get; set; }

		[JsonProperty(nameof(Countries))]
		public CountriesGunImportModel[] Countries { get; set; }
	}

	public class CountriesGunImportModel
	{
		[JsonProperty(nameof(Id))]
		public int Id { get; set; }
	}
}

