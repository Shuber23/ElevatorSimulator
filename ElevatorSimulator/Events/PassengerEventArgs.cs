using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Events
{
    public class PassengerEventArgs: EventArgs
    {
        public Passenger PassengerWhoRisedAnEvent;
        public ConcurrentBag<Passenger> PassengersWhichRisedAnEventConcurrentBag;

        public PassengerEventArgs(Passenger passengerWhoRisedAnEvent)
        {
            PassengerWhoRisedAnEvent = passengerWhoRisedAnEvent;
        }

        public PassengerEventArgs(ConcurrentBag<Passenger> passengersWhichRisedAnEventConcurrentBag)
        {
            PassengersWhichRisedAnEventConcurrentBag = passengersWhichRisedAnEventConcurrentBag;
        }
    }
}
