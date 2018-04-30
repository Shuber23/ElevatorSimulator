using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Abstract
{
    public interface IDispatcher
    {
        Manager PassengerManager { get; set; }
        Manager ElevatorManager { get; set; }
        Manager QueueManager { get; set; }

        object GetItem(Manager manager, States.Direction direction);

        void OnPassengerCalledElevator(PassengerEventArgs e);

        void CallElevator();
    }
}
