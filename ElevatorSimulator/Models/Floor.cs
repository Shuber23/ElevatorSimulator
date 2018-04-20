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
    class Floor
    {
        private readonly Dictionary<string, Button> buttons;
        private readonly IQueueManager queueManager;

        private event EventHandler OnElevatorArrived;
        private event EventHandler OnPassengerArrived;

        public Floor()
        {
            buttons.Add("UpCallButton", new UpCallButton());
            buttons.Add("DownCallButton", new DownCallButton());
            queueManager = new QueueManager();
        }
    }
}
