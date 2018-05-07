using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator
{
    class BuildingInitializator
    {
        internal void InitializeBuilding(int elevatorCount, int floorCount)
        {
            Building building = Building.GetInstance();
            building.CreateElevators(elevatorCount);
            building.CreateFloors(floorCount);
        }
    }
}
