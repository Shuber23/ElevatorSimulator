using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            BuildingInitializator buildingInitializator = new BuildingInitializator();
            ManagerInitializator managerInitializator = new ManagerInitializator();
            PassengerCreator passengerCreator = new PassengerCreator();

            Initializator initializator = new Initializator(buildingInitializator, managerInitializator, passengerCreator);

            initializator.Initialize();

            System.Threading.Thread.Sleep(600000);

        }
    }
}
