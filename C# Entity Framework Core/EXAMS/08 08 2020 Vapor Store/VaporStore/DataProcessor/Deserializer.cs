namespace VaporStore.DataProcessor
{
	using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
	{
		public static string ImportGames(VaporStoreDbContext context, string jsonString)
		{
			var sb = new StringBuilder();
			var games = JsonConvert.DeserializeObject<IEnumerable<GameJsonImportModel>>(jsonString);

            foreach (var game in games)
            {
				if (!IsValid(game))
                {
					sb.AppendLine("Invalid Data");
					continue;
                }
                if (game.Tags.Count() == 0)
                {
                    sb.AppendLine("Invalid Data");
                    continue;
                }

				var genre = context.Genres.FirstOrDefault(x => x.Name == game.Genre);
				if (genre == null)
                {
					genre = new Genre
					{
						Name = game.Genre
					};
                }

				var developer = context.Developers.FirstOrDefault(x => x.Name == game.Developer);
                if (developer == null)
                {
                    developer = new Developer
                    {
                        Name = game.Developer
                    };
                }

                var currentGame = new Game
				{
					Name = game.Name,
					Genre = genre,
					Developer = developer,
					Price = game.Price,
					ReleaseDate = game.ReleaseDate.Value,
				};
                foreach (var tag in game.Tags)
                {
					var currentTag = context.Tags.FirstOrDefault(x => x.Name == tag) ?? new Tag { Name = tag};
					currentGame.GameTags.Add(new GameTag { Tag = currentTag });
                }

				context.Games.Add(currentGame);
				context.SaveChanges();
				sb.AppendLine($"Added {game.Name} ({game.Genre}) with {game.Tags.Count()} tags");
            }

            return sb.ToString().TrimEnd();
        }

		public static string ImportUsers(VaporStoreDbContext context, string jsonString)
		{
            var sb = new StringBuilder();
			var users = JsonConvert.DeserializeObject<IEnumerable<UserJsonImportModel>>(jsonString);

            foreach (var jsonUser in users)
            {
				if (!IsValid(jsonUser))
                {
					sb.AppendLine("Invalid Data");
					continue;
                }

				var user = new User
				{
					FullName = jsonUser.FullName,
					Username = jsonUser.UserName,
					Age = jsonUser.Age,
					Email = jsonUser.Email,
					Cards = jsonUser.Cards.Select(x => new Card
					{
						Cvc = x.CVC,
						Number = x.Number,
						Type = x.Type.Value
					}).ToList(),
                };

				context.Users.Add(user);
				
				sb.AppendLine($"Imported {jsonUser.UserName} with {jsonUser.Cards.Count()} cards");

            }
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

		public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
		{
			throw new NotImplementedException();
		}

		private static bool IsValid(object dto)
		{
			var validationContext = new ValidationContext(dto);
			var validationResult = new List<ValidationResult>();

			return Validator.TryValidateObject(dto, validationContext, validationResult, true);
		}
	}
}