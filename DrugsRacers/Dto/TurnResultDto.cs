using DrugsRacers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Dto
{
    class TurnResultDto
    {
        public CommandModel Command { get; set; }

        public CellModel[] VisibleCells { get; set; }

        public PointModel Location { get; set; }

        public int ShortestWayLength { get; set; }

        public int Speed { get; set; }

        public string Status { get; set; }

        public string Heading { get; set; }

        public int FuelWaste { get; set; }
    }
}
