using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Model
{
    class MoveInfoModel
    {
        public string SelectedDirection { get; set; }

        public string What { get; set; }

        public string What1 { get; set; }

        public string What2 { get; set; }

        public override string ToString()
        {
            return $"selected dir: {SelectedDirection} <> what: {What}";
        }
    }
}
