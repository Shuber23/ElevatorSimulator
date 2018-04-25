using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Buttons;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Models
{
    internal class Floor
    {
        private readonly Dictionary<string, Button> buttons;

        private List<Passenger> goingUpPassengerQueue = new List<Passenger>();
        private List<Passenger> goingDownPassengerQueue = new List<Passenger>();
        public int floorIndex;

        private event EventHandler ElevatorArrived;
        private event EventHandler PassengerArrived;

        public Floor(int floorIndex)
        {
            this.floorIndex = floorIndex;
            buttons.Add("UpCallButton", new UpCallButton(floorIndex));
            buttons.Add("DownCallButton", new DownCallButton(floorIndex));
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
