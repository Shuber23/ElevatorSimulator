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
            Console.WriteLine("Passenger added to queue!");
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
            Console.WriteLine("Passenger removed from queue!");
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

        public override object GetItem(States.Direction direction)
        {
            throw new NotImplementedException();
        }
    }
}
