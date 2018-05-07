using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;

namespace ElevatorSimulator.Events
{

    public delegate void PassengerEventHandler(object sender, PassengerEventArgs e);
    public delegate void ElevatorEventHandler(object sender, ElevatorEventArgs e);

    static class GlobalEvents
    {
        public static event ElevatorEventHandler ElevatorArrived;
        public static event ElevatorEventHandler ElevatorUpdatedDirection;
        public static event PassengerEventHandler PassengerCalledElevator;
        public static event PassengerEventHandler PassengerEnteredElevator;
        public static event PassengerEventHandler PassengerReleasedElevator;

        private static object sender = new object();

        public static void OnElevatorUpdatedDirection(ElevatorEventArgs e)
        {
            ElevatorUpdatedDirection?.Invoke(sender, e);
        }

        public static void OnElevatorArrived(ElevatorEventArgs e)
        {
            ElevatorArrived?.Invoke(sender, e);
        }

        public static void OnPassengerCalledElevator(PassengerEventArgs e)
        {
            PassengerCalledElevator?.Invoke(sender, e);
        }

        public static void OnPassengerEnteredElevator(PassengerEventArgs e)
        {
            PassengerEnteredElevator?.Invoke(sender, e);
        }

        public static void OnPassengerReleasedElevator(PassengerEventArgs e)
        {
            PassengerReleasedElevator?.Invoke(sender, e);
        }
    }
}
