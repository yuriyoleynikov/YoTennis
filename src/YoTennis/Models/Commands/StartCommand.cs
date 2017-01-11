using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Commands
{
    public class StartCommand
    {
        public MatchSettings MatchSettings { get; set; }
        public string FirstPlayer { get; set; }
        public string SecondPlayer { get; set; }
    }
}
