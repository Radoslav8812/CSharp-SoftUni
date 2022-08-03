using System;
using System.Collections.Generic;

namespace VaporStore.Data.Models
{
    public class Genre
    {
        public Genre()
        {
            Games = new HashSet<Game>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<Game> Games { get; set; }

    }
}

