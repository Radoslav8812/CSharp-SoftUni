using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
	[XmlType("Country")]
	public class CountryImportModel
	{
		[StringLength(60, MinimumLength = 4)]
		[XmlElement(nameof(CountryName))]
		public string CountryName { get; set; }

		[Range(50000, 10000000)]
		[XmlElement(nameof(ArmySize))]
		public int ArmySize { get; set; }
	}
}

