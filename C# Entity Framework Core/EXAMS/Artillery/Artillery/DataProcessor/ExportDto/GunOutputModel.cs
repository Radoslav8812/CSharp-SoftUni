using System;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ExportDto
{
	[XmlType("Gun")]
	public class GunOutputModel
	{
		[XmlAttribute("Manufacturer")]
		public string Manufacturer { get; set; }

		[XmlAttribute("GunType")]
		public string GunType { get; set; }

        [XmlAttribute("GunWEight")]
        public string GunWeight { get; set; }

        [XmlAttribute("BarrelLength")]
        public string BarrelLength { get; set; }

		[XmlAttribute("Range")]
		public string Range { get; set; }

        [XmlArray("Countries")]
        public CountryOutputModel[] Countries { get; set; }
    }

	public class CountryOutputModel
	{
		[XmlAttribute("Country")]
		public string Country { get; set; }

        [XmlAttribute("ArmySize")]
        public int ArmySize { get; set; }

	}
}

