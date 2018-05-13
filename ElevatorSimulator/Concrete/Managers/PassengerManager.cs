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
    internal class PassengerManager: Manager
    {
        private IGenerator passengerGenerator;
        public PassengerManager(IDispatcher dispatcher, IGenerator generator) : base(dispatcher)
        {
            passengerGenerator = generator;
        }

        public override void Create()
        {
            Passenger passenger = passengerGenerator.Generate();
            CallElevator(passenger);
        }

        private void CallElevator(Passenger passenger)
        {
            Console.WriteLine("Passenger {0} created, appears om floor {1}, want to {2}, has weight {3} kg!", passenger.passengerIndex, passenger.CurrentFloorIndex, passenger.DestinationFloorIndex, passenger.Weight);
            UpdatePassengerDirection(passenger);
            GlobalEvents.OnPassengerCalledElevator(new PassengerEventArgs(passenger));
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
            //Console.WriteLine("Passenger {0} direction updated!", passenger.passengerIndex);
        }
    }
}
