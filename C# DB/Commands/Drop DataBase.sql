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