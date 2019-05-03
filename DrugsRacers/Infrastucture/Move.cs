using DrugsRacers.Dto;
using DrugsRacers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrugsRacers.Infrastucture
{
    class Move
    {
        static PlayerSessionInfoDto _info;

        static PointModel _currentLocation;

        static List<string> _freeDirections;

        static CellModel[] _neighbourCells;

        static List<PointModel> _previousPoints;

        static Dictionary<string, PointModel> _directionMap = new Dictionary<string, PointModel>
        {
            { "NorthEast", new PointModel(1, 0, -1) },
            { "East", new PointModel(1, -1, 0) },
            { "SouthEast", new PointModel(0, -1, 1) },
            { "SouthWest", new PointModel(-1, 0, 1) },
            { "West", new PointModel(-1, 1, 0) },
            { "NorthWest", new PointModel(0, 1, -1) }
        };

        public static void Init(PlayerSessionInfoDto sessionInfo)
        {
            _info = sessionInfo;
            _currentLocation = sessionInfo.CurrentLocation;
            _neighbourCells = sessionInfo.NeighbourCells;
            _previousPoints = new List<PointModel>() { sessionInfo.CurrentLocation };
        }

        public static void UpdatePosition(TurnResultDto turnInfo)
        {
            _currentLocation = turnInfo.Location;
            _neighbourCells = turnInfo.VisibleCells;
        }

        public static TurnResultDto Turn(string direction, int acceleration)
        {
            var turn = new TurnDto()
            {
                Acceleration = acceleration,
                Direction = direction
            };

            var result = Infra.Put<TurnResultDto>($"raceapi/race/{_info.SessionId}", turn, true);
            _previousPoints.Add(result.Location);
            return result;
        }

        public static string See(string direction)
        {
            var current = _currentLocation;
            var delta = _directionMap[direction];
            var targetLocation = new PointModel(current.X + delta.X, current.Y + delta.Y, current.Z + delta.Z);
            var targetCell = _neighbourCells.First(cll => cll.Item1.Equals(targetLocation));
            return targetCell.Item2;
        }

        public static PointModel GetFinishDdelta() =>
            GetFinishDdelta(_currentLocation);

        public static PointModel GetFinishDdelta(PointModel currentPoint)
        {
            var delta = new PointModel(currentPoint.X - _info.Finish.X, currentPoint.Y - _info.Finish.Y, currentPoint.Z - _info.Finish.Z);
            return delta;
        }

        public static string[] GetFinishDirection(PointModel delta)
        {
            var maxInfo = delta.GetMaxInfo();
            if (maxInfo.Item1 == "X")
            {
                if (maxInfo.Item2 > 0)
                    return new string[2] { "SouthWest", "West" };
                else
                    return new string[2] { "NorthEast", "East" };
            }
            else if (maxInfo.Item1 == "Y")
            {
                if (maxInfo.Item2 > 0)
                    return new string[2] { "East", "SouthEast" };
                else
                    return new string[2] { "West", "NorthWest" };
            }
            else
            {
                if (maxInfo.Item2 > 0)
                    return new string[2] { "NorthEast", "NorthWest" };
                else
                    return new string[2] { "SouthEast", "SouthWest" };
            }
        }

        public static MoveInfoModel GetBestDirection(string[] directions)
        {
            InitFreeDirections();

            var what1 = See(directions[0]);
            var what2 = See(directions[1]);

            var delta1 = _directionMap[directions[0]];
            var newLocation1 = new PointModel(_currentLocation.X + delta1.X, _currentLocation.Y + delta1.Y, _currentLocation.Z + delta1.Z);
            var delta2 = _directionMap[directions[1]];
            var newLocation2 = new PointModel(_currentLocation.X + delta2.X, _currentLocation.Y + delta2.Y, _currentLocation.Z + delta2.Z);

            var result = new MoveInfoModel()
            {
                What1 = what1,
                What2 = what2
            };

            if (what1 == "Empty" && !_previousPoints.Contains(newLocation1))
            {
                result.SelectedDirection = directions[0];
                result.What = what1;
                return result;
            }
            if (what2 == "Empty" && !_previousPoints.Contains(newLocation2))
            {
                result.SelectedDirection = directions[1];
                result.What = what2;
                return result;
            }

            result.SelectedDirection = GetNotRandomDirection();// GetRandomDirection();
            if (result.SelectedDirection == string.Empty)
            {
                _previousPoints.Clear();
                result.SelectedDirection = GetNotRandomDirection();
            }
            result.What = See(result.SelectedDirection);

            return result;
        }

        public static void InitFreeDirections()
        {
            _freeDirections = new List<string> { "NorthEast", "East", "SouthEast", "SouthWest", "West", "NorthWest" };
        }

        public static string GetNotRandomDirection()
        {
            var neighbours = new List<PointModel>();
            foreach (var dir in _directionMap)
                neighbours.Add(new PointModel(_currentLocation.X + dir.Value.X, _currentLocation.Y + dir.Value.Y, _currentLocation.Z + dir.Value.Z)
                { Direction = dir.Key });

            var allowCells = new List<CellModel>();
            foreach (var neighbour in neighbours)
            {
                if (_previousPoints.Contains(neighbour))
                    continue;

                var cell = _neighbourCells.First(cll => cll.Item1.Equals(neighbour));
                cell.Item1.Direction = neighbour.Direction;
                if (cell.Item2 == "Pit")
                {
                    allowCells.Add(cell);
                    cell.FinishDelta = GetFinishDdelta(cell.Item1);
                    cell.Wheigt = 1;
                }
                else if (cell.Item2 == "Empty" || cell.Item2 == "DangerousArea")
                {
                    allowCells.Add(cell);
                    cell.FinishDelta = GetFinishDdelta(cell.Item1);
                    cell.Wheigt = 0;
                }
            }

            var result = new List<CellModel>();

            var cools = allowCells.Where(cll => cll.Wheigt == 0);
            if (cools.Any())
            {
                cools.OrderBy(cll => cll.Item1.GetMaxInfo().Item2);
                result.AddRange(cools);
            }
            cools = allowCells.Where(cll => cll.Wheigt == 1);
            if (cools.Any())
            {
                cools.OrderBy(cll => cll.Item1.GetMaxInfo().Item2);
                result.AddRange(cools);
            }

            if (result.Count == 0)
                return string.Empty;

            var target = result.First();
            return target.Item1.Direction;


            //var normCELLS = neighbours
            //    .Select(nb  => _neighbourCells.First(cll => cll.Item1.Equals(nb)))
            //    .Where(nb =>
            //{
            //    return nb.Item2 == "Empty" || nb.Item2 == "DangerousArea";
            //}).ToList();

            //foreach(var cell in normCELLS)
            //{

            //}
            //return "";
        }

        public static string GetRandomDirection()
        {
            string firstDirection;
            while (true)
            {
                firstDirection = _freeDirections.First();
                _freeDirections.Remove(firstDirection);
                var current = _currentLocation;
                var delta = _directionMap[firstDirection];
                var targetLocation = new PointModel(current.X + delta.X, current.Y + delta.Y, current.Z + delta.Z);
                if (_previousPoints.Contains(targetLocation))
                    continue;
                var what = See(firstDirection);
                if (what != "Rock")
                    return firstDirection;
            }
            return string.Empty;
        }
    }
}
