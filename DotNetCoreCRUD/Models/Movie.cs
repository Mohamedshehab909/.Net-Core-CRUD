using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCRUD.Models
{
    public class Movie
    {
        public int Id { get; set; }
        [Required, MaxLength(250)]
        public string Titel { get; set; }
        public int Year { get; set; }
        public double Rate { get; set; }

        [Required , MaxLength(2500)]
        public string StoryLine { get; set; }

        [Required]
        public byte[] Poster { get; set; }

        public byte GenraId { get; set; }
        public Genra Genra { get; set; }
    }
}
