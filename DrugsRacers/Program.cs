using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DrugsRacers.Dto;
using DrugsRacers.Infrastucture;
using DrugsRacers.Model;

namespace DrugsRacers
{
    class Program
    {
        static TokenDto _token;

        static void Main(string[] args)
        {
            TurnResultDto newInfo = new TurnResultDto() { Status = "NotBad" };
            //try
            //{
            Infra.Init();

            var info = Infra.GetSessionInfo();
            Move.Init(info);

            Console.WriteLine("Ready to Go");
            Console.WriteLine($"Current location: {info.CurrentLocation}");

            var speed = 40;
            var acceleration = 30;

            while (newInfo.Status == "NotBad" || newInfo.Status == "Drifted")
            {
                var finishDelta = Move.GetFinishDdelta();
                //Console.WriteLine($"Finish Delta: {finishDelta}");

                var directions = Move.GetFinishDirection(finishDelta);
                //Console.WriteLine($"Finish Directions: {directions[0]} {directions[1]}");

                var bestDirection = Move.GetBestDirection(directions);

                if (bestDirection.What == "Pit" && speed < 70)
                    acceleration = 70 - speed;
                else if (bestDirection.What == "DangerousArea" && speed > 30)
                    acceleration = 30 - speed;
                else
                    acceleration = 40 - speed;

                Console.WriteLine($"Best Direction: {bestDirection} acceleration: {acceleration} speed: {speed} status: {newInfo.Status}");

                newInfo = Move.Turn(bestDirection.SelectedDirection, acceleration);
                speed = newInfo.Speed;

                Move.UpdatePosition(newInfo);
                Console.WriteLine($"New Location: {newInfo.Location}, status: {newInfo.Status}, fuel: {newInfo.FuelWaste}");
                Console.WriteLine("------------------------------------------------------------");

                Infra.Get(info.SessionId);

                //Console.ReadKey();



                //Console.WriteLine("Move or See");
                //var command = Console.ReadLine();
                //switch (command)
                //{
                //    case "See":
                //        See();
                //        break;

                //    case "Move":
                //        break;
                //}
                //var turnRes = Move.Turn("East", 1);
            }
            //}
            //catch
            //{
            //    var first = true;
            //    var step = 1;
            //    Console.WriteLine("Plan B");

            //    while (newInfo.Status == "Hangry")
            //    {
            //        if (step == 1)
            //        {
            //            newInfo = Move.Turn("West", first ? 1 : 0);
            //            step = 2;
            //        }
            //        else
            //        {
            //            newInfo = Move.Turn("East", first ? 1 : 0);
            //            step = 1;
            //        }


            //    }
            //}
            Console.ReadKey();
        }

        static void See()
        {
            Console.WriteLine("See: direction");
            var direction = Console.ReadLine();

            var result = Move.See(direction);

            Console.WriteLine(result);
            Console.WriteLine();
        }


    }
}

