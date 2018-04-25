using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    internal class Passenger
    {
        private readonly int weight;
        public PassengerState state;

        private int floorIndex;
        private int destinationFloorIndex;

        public Passenger(int passengerWeight, Direction direction, int floorIndex)
        {
            weight = passengerWeight;
            Direction = direction;
            this.floorIndex = floorIndex;
        }

        public int Weight { get; }

        public int CurrentFloorIndex { get; set; }

        public int DestinationFloorIndex { get; set; }

        public Direction Direction { get; set; }
    }
}
