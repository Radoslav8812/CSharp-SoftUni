using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace Artillery.DataProcessor.ImportDto
{
	[XmlType("Shell")]
	public class ShellsImportModel
	{

		[XmlElement(nameof(ShellWeight))]
		[Range(2, 1680)]
		public double ShellWeight { get; set; }


		[XmlElement(nameof(Caliber))]
		[StringLength(30, MinimumLength = 4)]
		public string Caliber { get; set; }

	}
}

