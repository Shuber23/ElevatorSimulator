using System;
using System.Collections.Concurrent;
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

        private ConcurrentBag<int> floorsToVisit;
        private List<Passenger> passengerInside;
        private int weightInside;
        private object locker = new object();
        public int elevatorIndex;

        internal ElevatorState state;

        public Elevator(int passengerCapacity, int weightCapacity, int elevatorIndex)
        {
            this.passengerCapacity = passengerCapacity;
            this.weightCapacity = weightCapacity;
            passengerInside = new List<Passenger>();
            state = ElevatorState.Waiting;
            CurrentFloorIndex = 1;
            floorsToVisit = new ConcurrentBag<int>();

            this.elevatorIndex = elevatorIndex;
        }

        public bool IsEmpty
        {
            get
            {
                lock (locker)
                {
                    return passengerInside.Count == 0;
                }
            }
        }

        public bool IsFull
        {
            get
            {
                lock (locker)
                {
                    return passengerInside.Count == passengerCapacity;
                }
            }
        }

        public ConcurrentBag<int> DestinationFloorIndexes
        {
            get
            {
                lock (locker)
                {
                    return floorsToVisit;
                }
            }
        }

        public int CurrentFloorIndex { get; set; }

        public int WeightInside
        {
            get
            {
                lock (locker)
                {
                    return weightInside;
                }
            }
        }

        public bool CanUseElevator(int incomingPassengerWeight)
            => weightInside + incomingPassengerWeight <= weightCapacity && !IsFull;

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

        internal List<Passenger> GetPeopleInside()
        {
            lock (locker)
            {
                return passengerInside;
            }
        }
    }
}
