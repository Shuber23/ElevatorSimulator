using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    internal class PassengerManager: Manager
    {
        public PassengerManager(IDispatcher dispatcher) : base(dispatcher)
        {
            
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
        }

        //public override Passenger GetItem<T>()
        //{
        //    return new Passenger(80, States.Direction.None, 0);
        //}

        //private Elevator FindAvailableElevator() => dispatcher
    }
}
