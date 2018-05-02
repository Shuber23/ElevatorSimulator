using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    public class Elevator
    {
        private readonly int passengerCapacity;
        private readonly int weightCapacity;

        private List<Floor> floorsToVisit;
        private List<Passenger> passengerInside;
        private int weightInside;
        private object locker = new object();

        internal ElevatorState state;

        public Elevator(int passengerCapacity, int weightCapacity)
        {
            this.passengerCapacity = passengerCapacity;
            this.weightCapacity = weightCapacity;
            passengerInside = new List<Passenger>();
            state = ElevatorState.Waiting;
            CurrentFloorIndex = 1;
            DestinationFloorIndexes = new List<int>();
        }

        public bool IsFull => passengerInside.Count == passengerCapacity;

        public List<int> DestinationFloorIndexes { get; set; }

        public int CurrentFloorIndex { get; set; }

        public bool CanUseElevator(int incomingPassengerWeight)
            => weightInside + incomingPassengerWeight <= weightInside && !IsFull;

        public void EnterInElevator(Passenger passenger)
        {
            lock (locker)
            {
                passengerInside.Add(passenger);
                weightInside += passenger.Weight;
            }
        }

        public void ExitFromElevator(Passenger passenger)
        {
            lock (locker)
            {
                passengerInside.Remove(passenger);
                weightInside -= passenger.Weight;
            }
        }

        internal List<Passenger> GetPeopleInsideList()
        {
            lock (locker)
            {
                return passengerInside;
            }
        }
    }
}
