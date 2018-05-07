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

        private static Building instance;

        private Building()
        {
            elevators = new List<Elevator>();
            floors = new List<Floor>();
        }

        internal static Building GetInstance()
        {
            if (instance == null)
            {
                instance = new Building();
            }
            return instance;
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
