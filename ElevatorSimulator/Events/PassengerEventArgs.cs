using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Events
{
    public class PassengerEventArgs: EventArgs
    {
        public Passenger passengerWhoRisedAnEvent;

        public PassengerEventArgs(Passenger passengerWhoRisedAnEvent)
        {
            this.passengerWhoRisedAnEvent = passengerWhoRisedAnEvent;
        }
    }
}
