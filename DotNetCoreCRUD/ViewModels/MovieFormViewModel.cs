using DotNetCoreCRUD.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreCRUD.ViewModels
{
    public class MovieFormViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(250)]
        public string Titel { get; set; }
        public int Year { get; set; }

        [Range (1,10)]
        public double Rate { get; set; }

        [Required, StringLength(2500)]
        public string StoryLine { get; set; }

       
        public byte[] Poster { get; set; }
        [Display (Name = "Genra")]
        public byte GenraId { get; set; }


        public IEnumerable<Genra> Genras { get; set; }
    }
}
