using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Model
{
    class PointModel
    {
        public PointModel() { }

        public PointModel(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public string Direction { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Z { get; set; }

        public Tuple<string, int> GetMaxInfo()
        {
            var xAbs = Math.Abs(X);
            var yAbs = Math.Abs(Y);
            var zAbs = Math.Abs(Z);

            if (xAbs > yAbs && xAbs > zAbs)
            {
                return new Tuple<string, int>("X", X);
            }
            else if (yAbs > xAbs && yAbs > zAbs)
            {
                return new Tuple<string, int>("Y", Y);
            }
            else
            {
                return new Tuple<string, int>("Z", Z);
            }
        }

        public override bool Equals(object obj)
        {
            var point = obj as PointModel;

            return point.X == X && point.Y == Y && point.Z == Z;
        }

        public override string ToString()
        {
            return $"x = {X}; y = {Y}; z = {Z}";
        }
    }
}
