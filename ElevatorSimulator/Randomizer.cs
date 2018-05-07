using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    public static class Randomizer
    {
        public static int currentFloor;
        public static int destinationFloor;
        public static int weight;
        public static int passengerIndex;

        public static void Random()
        {
            List<int> currentFloorIndexes = new List<int> {0, 1, 2, 3};
            List<int> destinationFloorIndexes = new List<int>{ 0, 1, 2, 3 };

            Random random = new Random();
            currentFloor = currentFloorIndexes[new Random().Next(0, currentFloorIndexes.Count)];
            destinationFloorIndexes.Remove(currentFloor);
            destinationFloor = destinationFloorIndexes[new Random().Next(0, destinationFloorIndexes.Count)];
            weight = random.Next(40, 131);
            passengerIndex++;
        }

       
    }
}
