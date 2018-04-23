using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            for (int i = 0; i < floorCount; i++)
            {
                floors.Add(new Floor());
            }

            for (int i = 0; i < elevatorCount; i++)
            {
                elevators.Add(new Elevator(4, 300));
            }
        }
    }
}
