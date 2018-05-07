using System;
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
        private List<Passenger> goingUpPassengerQueue = new List<Passenger>();
        private List<Passenger> goingDownPassengerQueue = new List<Passenger>();
        public int floorIndex;

        public Floor(int floorIndex)
        {
            this.floorIndex = floorIndex;
        }

        public List<Passenger> GoingUpPassengerQueue
        {
            get
            {
                return goingUpPassengerQueue;
            }
        }

        public List<Passenger> GoingDownPassengerQueue
        {
            get
            {
                return goingDownPassengerQueue;
            }
        }

    }
}
