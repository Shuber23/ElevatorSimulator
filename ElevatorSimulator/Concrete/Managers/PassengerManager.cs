using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    internal class PassengerManager: Manager, ICallElevator
    {
        public PassengerManager(IDispatcher dispatcher) : base(dispatcher)
        {
            
        }
        public void CallElevator(Passenger passenger)
        {
            Console.WriteLine("Passenger {0} created, appears om floor {1}, want to {2}, has weight {3} kg!", passenger.passengerIndex, passenger.CurrentFloorIndex, passenger.DestinationFloorIndex, passenger.Weight);
            UpdatePassengerDirection(passenger);
            OnPassengerCalledElevator(new PassengerEventArgs(passenger));
            //dispatcher.OnPassengerCalledElevator(new PassengerEventArgs(passenger));
        }

        private void UpdatePassengerDirection(Passenger passenger)
        {
            if (passenger.CurrentFloorIndex < passenger.DestinationFloorIndex)
            {
                passenger.Direction = States.Direction.Up;
            }
            else
            {
                passenger.Direction = States.Direction.Down;
            }
            Console.WriteLine("Passenger {0} direction updated!", passenger.passengerIndex);
        }

        public override object GetItem(States.Direction direction)
        {
            return new Passenger(80, States.Direction.None, 0, 3, 0);
        }

        private Elevator FindAvailableElevator(States.Direction direction) =>
            dispatcher.GetItem(this, direction) as Elevator;

        private void EnterTheElevator()
        {
            
        }
    }
}
