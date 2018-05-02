using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Models
{
    class Building
    {
        private readonly List<Elevator> elevators;
        private readonly List<Floor> floors;

        public Building(int elevatorCount, int floorCount)
        {
            elevators = new List<Elevator>();
            floors = new List<Floor>();

            for (int floorIndex = 0; floorIndex < floorCount; floorIndex++)
            {
                floors.Add(new Floor(floorIndex));
            }

            for (int elevatorIndex = 0; elevatorIndex < elevatorCount; elevatorIndex++)
            {
                elevators.Add(new Elevator(4, 300));
            }

            IDispatcher dispatcher = Dispatcher.GetInstance();
            Manager passengerManager = new PassengerManager(dispatcher);
            Manager elevatorManager = new ElevatorManager(dispatcher, elevators);
            Manager queueManager = new QueueManager(dispatcher, floors);

            dispatcher.PassengerManager = passengerManager;
            dispatcher.ElevatorManager = elevatorManager;
            dispatcher.QueueManager = queueManager;

            passengerManager.PassengerCalledElevator += dispatcher.PassengerCalledElevatorEventHandler;

            dispatcher.CallElevator();
            System.Threading.Thread.Sleep(120000);
        }

        //private void PassengerManagerOnPassengerCalledElevator(object sender, EventArgs eventArgs)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
