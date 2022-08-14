
namespace Artillery.DataProcessor
{
    using Artillery.Data;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportShells(ArtilleryContext context, double shellWeight)
        {
            var shells = context.Shells
                .Where(x => x.ShellWeight > shellWeight)
                .ToArray()
                .Select(x => new
                {
                    ShellWeight = x.ShellWeight,
                    Caliber = x.Caliber,
                    Guns = x.Guns.Where(x => x.GunType == GunType.AntiAircraftGun)
                    .Select(x => new
                    {
                        GunType = x.GunType.ToString(),
                        GunWeight = x.GunWeight,
                        BarrelLength = x.BarrelLength,
                        Range = x.Range > 3000 ? "Long-range" : "Regular range"
                    })
                    .OrderByDescending(x => x.GunWeight)
                    .ToArray()
                })
                .OrderBy(x => x.ShellWeight)
                .ToArray();

            return JsonConvert.SerializeObject(shells, Formatting.Indented);
        }

        public static string ExportGuns(ArtilleryContext context, string manufacturer)
        {
            var dtoGuns = context.Guns
                .Where(x => x.Manufacturer.ManufacturerName == manufacturer)
                .Select(x => new GunOutputModel
                {
                    Manufacturer = x.Manufacturer.ManufacturerName,
                    GunType = x.GunType.ToString(),
                    GunWeight = x.GunWeight.ToString(),
                    BarrelLength = x.BarrelLength.ToString(),
                    Range = x.Range.ToString(),
                    Countries = x.CountriesGuns.Where(x => x.Country.ArmySize > 4500000)
                    .Select(x => new CountryOutputModel
                    {
                        Country = x.Country.CountryName,
                        ArmySize = x.Country.ArmySize
                    })
                    .OrderBy(x => x.ArmySize)
                    .ToArray()
                })
                .OrderBy(x => x.BarrelLength)
                .ToArray();


            var xmlAttribute = new XmlRootAttribute("Guns");
            var xmlSerializer = new XmlSerializer(typeof(GunOutputModel[]), xmlAttribute);
            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);
            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "");

            xmlSerializer.Serialize(writer, dtoGuns, xmlSerializerNamespaces);

            return writer.ToString();
        }
    }
}
