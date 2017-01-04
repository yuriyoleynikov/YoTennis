using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Events
{
    public class ServeFailEvent : GameEvent
    {
        public ServeFailKind Serve { get; set; }
        public ServeSpeed ServeSpeed { get; set; }
    }
}