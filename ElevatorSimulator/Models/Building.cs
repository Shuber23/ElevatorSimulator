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

            IDispatcher dispatcher = Dispatcher.GetInstance(floors);
        }

        
    }
}
