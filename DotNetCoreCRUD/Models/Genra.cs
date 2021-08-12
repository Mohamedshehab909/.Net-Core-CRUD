﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace DotNetCoreCRUD.Models
{
    public class Genra
    {
        //عشان هو الى يدخل ال ادى عشان هو بايت
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public byte Id { get; set; }
        [Required , MaxLength(100)]
        public string Name { get; set; }
    }
}
