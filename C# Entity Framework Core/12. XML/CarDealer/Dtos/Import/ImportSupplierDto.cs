﻿using System;
using System.Xml.Serialization;

namespace CarDealer.Dtos.Import
{
    [XmlType("Supplier")]
    public class ImportSupplierDto
    {
        public ImportSupplierDto()
        {
        }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("isImporter")]
        public bool IsImporter { get; set; }
    }
}

