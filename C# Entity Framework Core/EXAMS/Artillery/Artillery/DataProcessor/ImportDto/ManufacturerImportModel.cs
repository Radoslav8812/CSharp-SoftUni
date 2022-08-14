using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
	[XmlType("Manufacturer")]
	public class ManufacturerImportModel
	{

		[StringLength(40, MinimumLength = 4)]
		[XmlElement(nameof(ManufacturerName))]
		public string ManufacturerName { get; set; }

        [StringLength(100, MinimumLength = 10)]
        [XmlElement(nameof(Founded))]
        public string Founded { get; set; }
	}
}

