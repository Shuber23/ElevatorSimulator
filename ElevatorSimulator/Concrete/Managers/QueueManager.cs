using System;
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

        private void AddToQueue(Passenger passenger)
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

            Console.WriteLine("Passenger {0} added to queue!", passenger.passengerIndex);
        }

        private void RemoveFromQueue(Passenger passenger)
        {
            lock (locker)
            {
                if (passenger.Direction == States.Direction.Down)
                {
                    floors[passenger.CurrentFloorIndex].GoingDownPassengerQueue.Remove(passenger);
                }
                else
                {
                    floors[passenger.CurrentFloorIndex].GoingUpPassengerQueue.Remove(passenger);
                }
            }

            Console.WriteLine("Passenger {0} removed from queue!", passenger.passengerIndex);
        }

        public void WorkWithQueue(Passenger passenger)
        {
            if (floors[passenger.CurrentFloorIndex].GoingUpPassengerQueue.Contains(passenger) ||
                floors[passenger.CurrentFloorIndex].GoingDownPassengerQueue.Contains(passenger))
            {
                RemoveFromQueue(passenger);
            }
            else
            {
                AddToQueue(passenger);
            }
        }

        public Passenger GetWaitingPassenger()
        {
            List<Passenger> allPassengers = new List<Passenger>();
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

                return allPassengers.Count > 0 ? allPassengers.OrderBy(x => x.passengerIndex).First(): null;
            }
        }

        public List<Passenger> GetAllPassengersOnFloorByDirection(int floorIndex, States.Direction direction)
        {
            lock (locker)
            {
                List<Passenger> allPassengers = new List<Passenger>();
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

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
