using System.IO;
using System.Linq;
using System.Xml.Serialization;
using CarDealer.Data;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext db = new CarDealerContext();
            //carDealerContext.Database.EnsureDeleted();
            //carDealerContext.Database.EnsureCreated();

            var xml = File.ReadAllText("../../../Datasets/suppliers.xml");

            var result = ImportSuppliers(db, xml);
            System.Console.WriteLine(result);
        }

        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");
            XmlSerializer serializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            ImportSupplierDto[] supplierDtos = (ImportSupplierDto[])serializer.Deserialize(reader);

            Supplier[] suppliers = supplierDtos.Select(dto => new Supplier()
            {
                Name = dto.Name,
                IsImporter = dto.IsImporter
            })
            .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Length}";
        }
    }
}