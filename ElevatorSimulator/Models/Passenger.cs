using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    public class Passenger
    {
        private readonly int weight;
        private PassengerState state;

        public Passenger(int passengerWeight)
        {
            weight = passengerWeight;
        }
    }
}
