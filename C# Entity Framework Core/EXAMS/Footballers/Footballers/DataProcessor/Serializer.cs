namespace Footballers.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Footballers.DataProcessor.ExportDto;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            var dtoCoaches = context.Coaches
                .ToArray()
                .Where(x => x.Footballers.Count > 0)
                .Select(x => new CoachOutputModel
                {
                    CoachName = x.Name,
                    FootballersCount = x.Footballers.Count,
                    Footballers = x.Footballers
                    .Select(x => new FootballerOfCoachOutputModel
                    {
                        Name = x.Name,
                        PositionType = x.PositionType.ToString()
                    })
                    .OrderBy(x => x.Name)
                    .ToArray()
                })
                .OrderByDescending(x => x.FootballersCount)
                .ThenBy(x => x.CoachName)
                .ToArray();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CoachOutputModel[]), xmlRootAttribute);
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            var sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);

            xmlSerializer.Serialize(writer, dtoCoaches, namespaces);
            return sb.ToString().TrimEnd();
        }


        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            var topTeam = context.Teams.ToArray()
                .Where(x => x.TeamsFootballers.Any(x => x.Footballer.ContractStartDate >= date))
                .Select(x => new
                {
                    Name = x.Name,
                    Footballers = x.TeamsFootballers
                    .OrderByDescending(x => x.Footballer.ContractEndDate)
                    .ThenBy(x => x.Footballer.Name)
                    .Where(x => x.Footballer.ContractStartDate >= date)
                    .Select(x => new
                    {
                        FootballerName = x.Footballer.Name,
                        ContractStartDate = x.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                        ContractEndDate = x.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                        BestSkillType = x.Footballer.BestSkillType.ToString(),
                        PositionType = x.Footballer.PositionType.ToString()
                    })
                    .ToArray()
                })
                .OrderByDescending(x => x.Footballers.Count())
                .ThenBy(x => x.Name)
                .Take(5)
                .ToArray();

            return JsonConvert.SerializeObject(topTeam, Formatting.Indented);
        }
    }
}
