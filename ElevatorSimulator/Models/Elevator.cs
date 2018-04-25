using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    internal class Elevator
    {
        private readonly int passengerCapacity;
        private readonly int weightCapacity;

        private List<Floor> floorsToVisit;
        private List<Passenger> peopleInside;
        private int weightInside;

        internal ElevatorState state;

        public Elevator(int passengerCapacity, int weightCapacity)
        {
            this.passengerCapacity = passengerCapacity;
            this.weightCapacity = weightCapacity;
            state = ElevatorState.Waiting;
            CurrentFloorIndex = 0;
        }

        public bool IsFull => floorsToVisit.Count == passengerCapacity;

        public Floor DestinationFloor { get; set; }

        public int CurrentFloorIndex { get; set; }

        public bool IsWeightAvaliable(int incomingPassengerWeight) =>
            weightInside + incomingPassengerWeight <= weightInside;


    }
}
