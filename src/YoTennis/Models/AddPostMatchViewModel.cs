﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class AddPostMatchViewModel
    {
        [Required, Display(Name = "Date"), DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [Display(Name = "Time"), DataType(DataType.Time)]
        public DateTime? Time { get; set; }

        [Required, Display(Name = "First Player Name")]
        public string FirstPlayer { get; set; }

        [Required, Display(Name = "Second Player Name")]
        public string SecondPlayer { get; set; }

        [Display(Name = "It's me")]
        public bool FirstPlayerUserId { get; set; }

        [Display(Name = "It's me")]
        public bool SecondPlayerUserId { get; set; }

        [Required, Display(Name = "First Player Set 1")]
        public int FirstPlayerSet1 { get; set; }

        [Required, Display(Name = "Second Player Set 1")]
        public int SecondPlayerSet1 { get; set; }

        [Display(Name = "First Player Set 2")]
        public int? FirstPlayerSet2 { get; set; }

        [Display(Name = "Second Player Set 2")]
        public int? SecondPlayerSet2 { get; set; }

        [Display(Name = "First Player Set 3")]
        public int? FirstPlayerSet3 { get; set; }

        [Display(Name = "Second Player Set 3")]
        public int? SecondPlayerSet3 { get; set; }

        [Display(Name = "First Player Set 4")]
        public int? FirstPlayerSet4 { get; set; }

        [Display(Name = "Second Player Set 4")]
        public int? SecondPlayerSet4 { get; set; }

        [Display(Name = "First Player Set 5")]
        public int? FirstPlayerSet5 { get; set; }

        [Display(Name = "Second Player Set 5")]
        public int? SecondPlayerSet5 { get; set; }
    }
}
