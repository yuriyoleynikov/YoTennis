using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Commands
{
    public class DrawCommand
    {
        [Required, Display(Name = "Player On Left")]
        public Player PlayerOnLeft { get; set; }
        [Required, Display(Name = "Player Serves")]
        public Player PlayerServes { get; set; }
    }
}
