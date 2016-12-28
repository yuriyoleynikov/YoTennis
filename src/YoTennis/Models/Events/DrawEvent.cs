using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Events
{
    public class DrawEvent : GameEvent
    {
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
    }
}
