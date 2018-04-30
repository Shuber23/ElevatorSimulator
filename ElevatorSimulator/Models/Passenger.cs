using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    public class Passenger
    {
        private readonly int weight;
        public PassengerState state;

        private int floorIndex;
        private int destinationFloorIndex;

        public Passenger(int passengerWeight, Direction direction, int floorIndex, int destinationFloorIndex)
        {
            weight = passengerWeight;
            Direction = direction;
            this.floorIndex = floorIndex;
            this.destinationFloorIndex = destinationFloorIndex;
        }

        public int Weight { get; }

        public int CurrentFloorIndex
        {
            get => floorIndex;
            set => floorIndex = value;

        }

        public int DestinationFloorIndex
        {
            get => destinationFloorIndex;
            set => destinationFloorIndex = value;
        }

        public Direction Direction { get; set; }
    }
}
