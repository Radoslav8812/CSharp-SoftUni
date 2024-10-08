USE master;
GO

ALTER DATABASE SoftUni SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
GO
DROP DATABASE SoftUni;
GO

StringBuilder sb = new StringBuilder();

           
            ImportTheatresWithTicketsDto[] ticketsDtos =
                JsonConvert.DeserializeObject<ImportTheatresWithTicketsDto[]>(jsonString);

            ICollection<Theatre> validTheatres = new List<Theatre>();

            foreach (var tDto in ticketsDtos)
            {
                if (!IsValid(tDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Theatre theatre = new Theatre()
                {
                    Name = tDto.Name,
                    NumberOfHalls = tDto.NumberOfHalls,
                    Director = tDto.Director,
                };

                foreach (var dto in tDto.Tickets)
                {
                    if (!IsValid(dto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket()
                    {
                        Price = dto.Price,
                        RowNumber = dto.RowNumber,
                        PlayId = dto.PlayId
                    };

                    theatre.Tickets.Add(ticket);
                }

                validTheatres.Add(theatre);
                sb.AppendLine(String.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));
            }

            context.Theatres.AddRange(validTheatres);
            context.SaveChanges();

            return sb.ToString().TrimEnd();

            XmlRootAttribute xmlRootAttribute = new XmlRootAttribute("Coaches");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CoachWithFootballersInputModel[]), xmlRootAttribute);

            using StringReader reader = new StringReader(xmlString);

            CoachWithFootballersInputModel[] dtoCoaches = (CoachWithFootballersInputModel[])xmlSerializer.Deserialize(reader);

            StringBuilder sb = new StringBuilder();
            List<Coach> validCoaches = new List<Coach>();

            foreach (CoachWithFootballersInputModel dtoCoach in dtoCoaches)
            {
                if (!IsValid(dtoCoach) || String.IsNullOrEmpty(dtoCoach.Nationality) || DateTime.TryParse(dtoCoach.Nationality, out DateTime dt) == true)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Coach newCoach = new Coach()
                {
                    Name = dtoCoach.Name,
                    Nationality = dtoCoach.Nationality,

                };

                List<Footballer> validFootballers = new List<Footballer>();
                foreach (FootballerOfCoachInputModel dtoFootballer in dtoCoach.Footballers)
                {
                    if (!IsValid(dtoFootballer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    bool IsValidContractStartDate = DateTime.TryParseExact(dtoFootballer.ContractStartDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractStarDate);

                    bool IsValidContractEndDate = DateTime.TryParseExact(dtoFootballer.ContractEndDate,
                        "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime contractEndDate);

                    if (!IsValidContractStartDate || !IsValidContractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (contractStarDate > contractEndDate)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Footballer newFootballer = new Footballer()
                    {
                       Name = dtoFootballer.Name,
                       ContractStartDate = DateTime.ParseExact(dtoFootballer.ContractStartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       ContractEndDate = DateTime.ParseExact(dtoFootballer.ContractEndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                       BestSkillType = Enum.Parse<BestSkillType>(dtoFootballer.BestSkillType),
                       PositionType = Enum.Parse<PositionType>(dtoFootballer.PositionType)
                    };

                    validFootballers.Add(newFootballer);

                }
                newCoach.Footballers = validFootballers;
                validCoaches.Add(newCoach);
                sb.AppendLine(String.Format(SuccessfullyImportedCoach, newCoach.Name, newCoach.Footballers.Count));
            }
            context.Coaches.AddRange(validCoaches);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            TeamInputModel[] dtoTeams = JsonConvert.DeserializeObject<TeamInputModel[]>(jsonString);

            StringBuilder sb = new StringBuilder();

            List<Team> validTeams = new List<Team>();

            foreach (TeamInputModel dtoTeam in dtoTeams)
            {
                if (!IsValid(dtoTeam) || dtoTeam.Trophies <= 0)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                //Team newTeam = new Team
                //{
                //    Name = dtoTeam.Name,
                //    Nationality = dtoTeam.Nationality,
                //    Trophies = dtoTeam.Trophies
                //};

                //With AutoMapper

                //Team newTeam = Mapper.Map<Team>(dtoTeam);

                InitializeAutoMapper();
                Team newTeam = mapper.Map<Team>(dtoTeam);


                List<int> validFootballersIds = context.Footballers.Select(f => f.Id).ToList();
              

                foreach (int dtoFootballerId in dtoTeam.Footballers.Distinct())
                {
                    if (!validFootballersIds.Contains(dtoFootballerId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    newTeam.TeamsFootballers.Add(new TeamFootballer
                    {
                        FootballerId = dtoFootballerId,
                       Team = newTeam
                    });

                    //With AutoMappe

                    Footballer footballer = context.Footballers.Find(dtoFootballerId);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }


                    //TeamFootballer teamFootballer = Mapper.Map<TeamFootballer>(footballer);

                    TeamFootballer teamFootballer = mapper.Map<TeamFootballer>(footballer);
                    teamFootballer.Team = newTeam;

                    newTeam.TeamsFootballers.Add(teamFootballer);
                }

                validTeams.Add(newTeam);
                sb.AppendLine(String.Format(SuccessfullyImportedTeam, newTeam.Name, newTeam.TeamsFootballers.Count));

            }
            context.AddRange(validTeams);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }