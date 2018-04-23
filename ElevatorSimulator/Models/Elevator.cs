using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ElevatorSimulator.States;

namespace ElevatorSimulator.Models
{
    class Elevator
    {
        private readonly int passengerCapacity;
        private readonly int weightCapacity;
        private ElevatorState state;

        public Elevator(int passengerCapacity, int weightCapacity)
        {
            this.passengerCapacity = passengerCapacity;
            this.weightCapacity = weightCapacity;
        }
    }
}
