using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    class QueueManager: IQueueManager
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
    }
}
