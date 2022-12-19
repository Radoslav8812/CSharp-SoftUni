using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Watchlist.Data.Models;

namespace Watchlist.Models
{
	public class MovieViewModel
	{
        
        public int Id { get; set; }

        public string Title { get; set; }
    
        public string Director { get; set; }
        
        public string ImageUrl { get; set; }

        public decimal Rating { get; set; }

        public string Genre { get; set; }
    }
}

