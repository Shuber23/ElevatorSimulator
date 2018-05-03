using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;
using Timer = System.Timers.Timer;

namespace ElevatorSimulator.Concrete.Managers
{
    class Dispatcher: IDispatcher
    {
        private static Dispatcher instance;
        public Manager QueueManager { get; set; }
        public Manager ElevatorManager { get; set; }
        public Manager PassengerManager { get; set; }

        public delegate void EventHandler(object sender, EventArgs e);
        
        //private readonly List<Elevator> elevators;
        private Timer floorChecker;

        private readonly object locker = new object();

        public static Dispatcher GetInstance()
        {
            if (instance == null)
            {
                instance = new Dispatcher();
            }
            return instance;
        }

        private Dispatcher()
        {

        }

        public object GetItem(Manager manager, States.Direction direction = States.Direction.None)
        {
            if (ElevatorManager == manager)
            {
                return ElevatorManager.GetItem(direction);
            }
            if (PassengerManager == manager)
            {
                return ElevatorManager.GetItem(direction);
            }
            return null;
        }

        public void CallElevator(int passengerIndex)
        {
            Randomizer.Random();
            ((ICallElevator) PassengerManager).CallElevator(new Passenger(Randomizer.weight, States.Direction.None,
                Randomizer.currentFloor, Randomizer.destinationFloor, passengerIndex));
        }

        public void PassengerCalledElevatorEventHandler(object sender, PassengerEventArgs e)
        {
            Console.WriteLine("Passenger {0} called elevator!", e.PassengerWhoRisedAnEvent.passengerIndex);
            ThreadPool.QueueUserWorkItem(delegate { ((IQueue)QueueManager).WorkWithQueue(e.PassengerWhoRisedAnEvent); });
            ThreadPool.QueueUserWorkItem(delegate { ((IMovable)ElevatorManager).SendElevatorForPassenger(e.PassengerWhoRisedAnEvent); });
        }

        public void PassengerEnteredElevatorEventHandler(object sender, PassengerEventArgs e)
        {
            Console.WriteLine("Passenger {0} entered the elevator!", e.PassengerWhoRisedAnEvent.passengerIndex);
            ThreadPool.QueueUserWorkItem(delegate { ((IQueue)QueueManager).WorkWithQueue(e.PassengerWhoRisedAnEvent); });
        }

        public void ElevatorArrivedEventHandler(object sender, ElevatorEventArgs e)
        {
            foreach (var destinationFloorIndex in e.elevatorWhoRisedAnEvent.DestinationFloorIndexes)
            {
                if (destinationFloorIndex == e.elevatorWhoRisedAnEvent.CurrentFloorIndex)
                {
                    
                }
            }
        }
    }
}
