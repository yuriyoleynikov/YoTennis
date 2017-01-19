using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YoTennis.Models.Commands
{
    public class ServeFailCommand
    {
        public ServeFailKind Serve { get; set; }
        public ServeSpeed ServeSpeed { get; set; }
    }
}