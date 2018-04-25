using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    class QueueManager: Manager
    {
        private readonly List<Floor> floors;

        public QueueManager(IDispatcher dispatcher, List<Floor> floors) : base(dispatcher) => this.floors = floors;

        public void AddToQueue(Passenger passenger, int floorIndex, States.Direction direction)
        {
            if (direction == States.Direction.Down)
            {
                floors[floorIndex].GoingDownPassengerQueue.Add(passenger);
            }
            else
            {
                floors[floorIndex].GoingUpPassengerQueue.Add(passenger);
            }
        }

        public void RemoveFromQueue(Passenger passenger, int floorIndex, States.Direction direction)
        {
            if (direction == States.Direction.Down)
            {
                floors[floorIndex].GoingDownPassengerQueue.Remove(passenger);
            }
            else
            {
                floors[floorIndex].GoingUpPassengerQueue.Remove(passenger);
            }
        }
    }
}
