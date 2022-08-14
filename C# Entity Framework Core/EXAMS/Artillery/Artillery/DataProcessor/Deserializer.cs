namespace Artillery.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Artillery.Data;
    using Artillery.Data.Models;
    using Artillery.Data.Models.Enums;
    using Artillery.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage =
                "Invalid data.";
        private const string SuccessfulImportCountry =
            "Successfully import {0} with {1} army personnel.";
        private const string SuccessfulImportManufacturer =
            "Successfully import manufacturer {0} founded in {1}.";
        private const string SuccessfulImportShell =
            "Successfully import shell caliber #{0} weight {1} kg.";
        private const string SuccessfulImportGun =
            "Successfully import gun {0} with a total weight of {1} kg. and barrel length of {2} m.";

        public static string ImportCountries(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Countries");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CountryImportModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);
            var dtoModels = (CountryImportModel[])xmlSerializer.Deserialize(reader);

            var sb = new StringBuilder();
            var resultCountryList = new List<Country>();

            foreach (var dtoCountry in dtoModels)
            {
                if (!IsValid(dtoCountry))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newCountry = new Country()
                {
                    CountryName = dtoCountry.CountryName,
                    ArmySize = dtoCountry.ArmySize
                };

                resultCountryList.Add(newCountry);
                sb.AppendLine(String.Format(SuccessfulImportCountry, newCountry.CountryName, newCountry.ArmySize));
            }

            context.Countries.AddRange(resultCountryList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportManufacturers(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Manufacturers");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ManufacturerImportModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);
            var dtoModels = (ManufacturerImportModel[])xmlSerializer.Deserialize(reader);

            var sb = new StringBuilder();
            var resultManufacturerList = new List<Manufacturer>();

            foreach (var dtoManu in dtoModels)
            {
                if (!IsValid(dtoManu))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var manuNameExist = resultManufacturerList.FirstOrDefault(x => x.ManufacturerName == dtoManu.ManufacturerName) != null;
                if (manuNameExist)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var founded = dtoManu.Founded.Split(", ");
                if (founded.Length < 3)
                {
                    continue;
                }

                var newManufacturer = new Manufacturer()
                {
                    ManufacturerName = dtoManu.ManufacturerName,
                    Founded = dtoManu.Founded
                };

                resultManufacturerList.Add(newManufacturer);

                var splitedNameAndCountry = $"{founded[founded.Length - 2]}, {founded[founded.Length - 1]}";

                sb.AppendLine(String.Format(SuccessfulImportManufacturer, newManufacturer.ManufacturerName, splitedNameAndCountry));
            }
            context.Manufacturers.AddRange(resultManufacturerList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportShells(ArtilleryContext context, string xmlString)
        {
            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Shells");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ShellsImportModel[]), xmlRootAttribute);
            using StringReader reader = new StringReader(xmlString);
            var dtoModels = (ShellsImportModel[])xmlSerializer.Deserialize(reader);

            var sb = new StringBuilder();
            var resultShellList = new List<Shell>();

            foreach (var dtoShell in dtoModels)
            {
                if (!IsValid(dtoShell))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newShell = new Shell()
                {
                    ShellWeight = dtoShell.ShellWeight,
                    Caliber = dtoShell.Caliber
                };

                resultShellList.Add(newShell);
                sb.AppendLine($"Successfully import shell caliber #{newShell.Caliber} weight {newShell.ShellWeight} kg.");
            }

            context.Shells.AddRange(resultShellList);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportGuns(ArtilleryContext context, string jsonString)
        {
            var dtoModels = JsonConvert.DeserializeObject<GunImportModel[]>(jsonString);
            var sb = new StringBuilder();
            var resultGunList = new List<Gun>();

            foreach (var dtoGun in dtoModels)
            {
                if (!IsValid(dtoGun))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var newGun = new Gun()
                {
                    ManufacturerId = dtoGun.ManufacturerId,
                    GunWeight = dtoGun.GunWeight,
                    BarrelLength = dtoGun.BarrelLength,
                    NumberBuild = dtoGun.NumberBuild,
                    ShellId = dtoGun.ShellId,
                    Range = dtoGun.Range,
                    GunType = Enum.Parse<GunType>(dtoGun.GunType),
                    CountriesGuns = dtoGun.Countries
                    .Select(x => new CountryGun
                    {
                        CountryId = x.Id
                    })
                    .ToArray()
                };

                resultGunList.Add(newGun);
                sb.AppendLine($"Successfully import gun {newGun.GunType} with a total weight of {newGun.GunWeight} kg. and barrel length of {newGun.BarrelLength} m.");
            }

            context.Guns.AddRange(resultGunList);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }
        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
