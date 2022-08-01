using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SoftJail.Common;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentCellDto
    {
        public ImportDepartmentCellDto()
        {
        }

        [Range(ValidationConstants.CellNumberMinValue, ValidationConstants.CellNumberMaxValue)]
        [JsonProperty(nameof(CellNumber))]
        public int CellNumber { get; set; }

        [JsonProperty(nameof(HasWindow))]
        public bool HasWindow { get; set; }
    }
}

