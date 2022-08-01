using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SoftJail.Common;

namespace SoftJail.DataProcessor.ImportDto
{
    public class ImportDepartmentWithCells
    {
        public ImportDepartmentWithCells()
        {
        }

        [Required]
        [MinLength(ValidationConstants.DepartmentNameMinLength)]
        [MaxLength(ValidationConstants.DepartmentNameMaxLength)]
        [JsonProperty(nameof(Name))]
        public string Name { get; set; }

        [JsonProperty(nameof(Cells))]
        public ImportDepartmentCellDto[] Cells { get; set; }
    }
}

