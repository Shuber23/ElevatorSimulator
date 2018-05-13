using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    class QueueManager: Manager, IQueue
    {
        private readonly List<Floor> floors;
        private object locker = new object();

        public QueueManager(IDispatcher dispatcher, List<Floor> floors) : base(dispatcher) => this.floors = floors;

        public void AddToQueue(Passenger passenger)
        {
            lock (locker)
            {
                if (passenger.Direction == States.Direction.Down)
                {
                    floors[passenger.CurrentFloorIndex].GoingDownPassengerQueue.Add(passenger);
                }
                else
                {
                    floors[passenger.CurrentFloorIndex].GoingUpPassengerQueue.Add(passenger);
                }
            }
        }

        public void RemoveFromQueue(Passenger passenger)
        {
            lock (locker)
            {
                if (passenger.Direction == States.Direction.Down)
                {
                    floors[passenger.CurrentFloorIndex].GoingDownPassengerQueue.TryTake(out passenger);
                }
                else
                {
                    floors[passenger.CurrentFloorIndex].GoingUpPassengerQueue.TryTake(out passenger);
                }
            }
            //Console.WriteLine("Passenger {0} removed from queue", passenger.passengerIndex);
        }

        public Passenger GetWaitingPassenger()
        {
            ConcurrentBag<Passenger> allPassengers = new ConcurrentBag<Passenger>();
            lock (locker)
            {
                foreach (var floor in floors)
                {
                    if (floor.GoingUpPassengerQueue.Any())
                    {
                        allPassengers.Add(floor.GoingUpPassengerQueue.OrderBy(x => x.passengerIndex).First());
                    }

                    if (floor.GoingDownPassengerQueue.Any())
                    {
                        allPassengers.Add(floor.GoingDownPassengerQueue.OrderBy(x => x.passengerIndex).First());
                    }
                }
                Passenger passenger = allPassengers.Count > 0 ? allPassengers.OrderBy(x => x.passengerIndex).First() : null;
                if (passenger != null)
                {
                    RemoveFromQueue(passenger);
                }
                return passenger;
            }
        }

        public ConcurrentBag<Passenger> GetAllPassengersOnFloorByDirection(int floorIndex, States.Direction direction)
        {
            lock (locker)
            {
                ConcurrentBag<Passenger> allPassengers = new ConcurrentBag<Passenger>();
                switch (direction)
                {
                    case States.Direction.Up:
                        allPassengers = floors[floorIndex].GoingUpPassengerQueue;
                        return allPassengers;
                    case States.Direction.Down:
                        allPassengers = floors[floorIndex].GoingDownPassengerQueue;
                        return allPassengers;
                    default:
                        throw new ArgumentOutOfRangeException("Invalid direction!");
                }
            }
        }

        public List<ConcurrentBag<Passenger>> GetAllPassengers()
        {
            List<ConcurrentBag<Passenger>> allPassengers = new List<ConcurrentBag<Passenger>>();
            for (int i=0;i<floors.Count;i++)
            {
                ConcurrentBag<Passenger> passengersGoingUpOnFloor = new ConcurrentBag<Passenger>();
                ConcurrentBag<Passenger> passengersGoingDownOnFloor = new ConcurrentBag<Passenger>();
                lock (locker)
                {
                    passengersGoingDownOnFloor = floors[i].GoingDownPassengerQueue;
                    passengersGoingUpOnFloor = floors[i].GoingUpPassengerQueue;
                }
                allPassengers.Add(passengersGoingUpOnFloor);
                allPassengers.Add(passengersGoingDownOnFloor);
            }

            return allPassengers;
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
