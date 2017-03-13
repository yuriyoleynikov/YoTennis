using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Commands
{
    public class ClickCommand
    {
        public Player PlayerPoint { get; set; }
        public PointKind Kind { get; set; }
        public ServeSpeed ServeSpeed { get; set; }
    }
}