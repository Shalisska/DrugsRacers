using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Model
{
    class CommandModel
    {
        public PointModel Location { get; set; }

        public int Acceleration { get; set; }

        public string MovementDirection { get; set; }

        public string Heading { get; set; }

        public int Speed { get; set; }

        public int Fuel { get; set; }
    }
}
