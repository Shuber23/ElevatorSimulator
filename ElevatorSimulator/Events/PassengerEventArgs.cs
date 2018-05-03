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
        public Passenger PassengerWhoRisedAnEvent;
        public List<Passenger> PassengersWhichRisedAnEventList;

        public PassengerEventArgs(Passenger passengerWhoRisedAnEvent)
        {
            PassengerWhoRisedAnEvent = passengerWhoRisedAnEvent;
        }

        public PassengerEventArgs(List<Passenger> passengersWhichRisedAnEventList)
        {
            PassengersWhichRisedAnEventList = passengersWhichRisedAnEventList;
        }
    }
}
