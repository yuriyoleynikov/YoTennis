using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models
{
    public class AddPostMatchViewModel
    {
        [Required, Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Required, Display(Name = "First Player Name")]
        public string FirstPlayer { get; set; }

        [Required, Display(Name = "Second Player Name")]
        public string SecondPlayer { get; set; }

        [Display(Name = "First Player UserId")]
        public bool FirstPlayerUserId { get; set; }

        [Display(Name = "Second Player UserId")]
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
    }
}
