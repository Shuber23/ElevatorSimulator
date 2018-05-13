using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Models
{
    internal class Floor
    {
        private ConcurrentBag<Passenger> goingUpPassengerQueue = new ConcurrentBag<Passenger>();
        private ConcurrentBag<Passenger> goingDownPassengerQueue = new ConcurrentBag<Passenger>();
        public int floorIndex;

        public Floor(int floorIndex)
        {
            this.floorIndex = floorIndex;
        }

        public ConcurrentBag<Passenger> GoingUpPassengerQueue
        {
            get
            {
                return goingUpPassengerQueue;
            }
        }

        public ConcurrentBag<Passenger> GoingDownPassengerQueue
        {
            get
            {
                return goingDownPassengerQueue;
            }
        }

    }
}
