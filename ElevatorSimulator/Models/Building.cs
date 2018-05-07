using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Models
{
    class Building
    {
        private readonly List<Elevator> elevators;
        private readonly List<Floor> floors;

        internal List<Elevator> Elevators => elevators;
        internal List<Floor> Floors => floors;

        public Building(int elevatorCount, int floorCount)
        {
            elevators = new List<Elevator>();
            floors = new List<Floor>();


            Manager passengerManager = new PassengerManager(dispatcher, new PassengerGenerator());
            Manager elevatorManager = new ElevatorManager(dispatcher, elevators);
            Manager queueManager = new QueueManager(dispatcher, floors);

            dispatcher.PassengerManager = passengerManager;
            dispatcher.ElevatorManager = elevatorManager;
            dispatcher.QueueManager = queueManager;

            passengerManager.PassengerCalledElevator += dispatcher.PassengerCalledElevatorEventHandler;
            elevatorManager.PassengerEnteredElevator += dispatcher.PassengerEnteredElevatorEventHandler;
            elevatorManager.PassengerReleasedElevator += dispatcher.PassengerReleasedElevatorEventHandler;

            int i = 0;
            while (i < 10)
            {
                passengerManager.Create();
                i++;
                System.Threading.Thread.Sleep(100);
            }

            System.Threading.Thread.Sleep(600000);
            Console.ReadLine();
        }

        internal void CreateFloors(int floorCount)
        {
            for (int floorIndex = 0; floorIndex < floorCount; floorIndex++)
            {
                floors.Add(new Floor(floorIndex));
            }
        }

        internal void CreateElevators(int elevatorCount)
        {
            for (int elevatorIndex = 0; elevatorIndex < elevatorCount; elevatorIndex++)
            {
                elevators.Add(new Elevator(4, 300, elevatorIndex));
            }
        }
    }
}
