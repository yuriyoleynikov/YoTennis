﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Commands
{
    public class DrawCommand
    {
        public Player PlayerOnLeft { get; set; }
        public Player PlayerServes { get; set; }
    }
}
