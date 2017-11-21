using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Events
{
    public class DeletePlayersEvent : GameEvent
    {
        public bool FirstPlayerUserId { get; set; } = false;
        public bool SecondPlayerUserId { get; set; } = false;
    }
}
