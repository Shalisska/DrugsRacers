using DrugsRacers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Dto
{
    class PlayerSessionInfoDto
    {
        public string SessionId { get; set; }

        public string PlayerId { get; set; }

        public string CurrentDirection { get; set; }

        public PointModel CurrentLocation { get; set; }

        public PointModel Finish { get; set; }

        public int Radius { get; set; }

        public int CurrentSpeed { get; set; }

        public string PlayerStatus { get; set; }

        public CellModel[] NeighbourCells { get; set; }

        public int Fuel { get; set; }
       
    }
}
