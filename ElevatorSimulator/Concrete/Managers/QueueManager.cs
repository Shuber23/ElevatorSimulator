using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    public class QueueManager: IQueueManager
    {
        private List<Passenger> goingUpPassengerQueue;
        private List<Passenger> goingDownPassengerQueue;
        //private r object locked;

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

        public void AddToQueue(Passenger passenger)
        {
            if (goingDownPassengerQueue == null)
            {
                goingDownPassengerQueue = new List<Passenger>();
            }
            goingDownPassengerQueue.Add(passenger);
        }

        public void RemoveFromQueue(Passenger passenger)
        {
            if (goingDownPassengerQueue == null)
            {
                goingDownPassengerQueue = new List<Passenger>();
            }
            goingDownPassengerQueue.Add(passenger);
        }
    }
}
