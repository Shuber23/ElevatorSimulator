using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Events
{
    public class ElevatorEventArgs
    {
        public Elevator elevatorWhoRisedAnEvent;

        public ElevatorEventArgs(Elevator elevatorWhoRisedAnEvent)
        {
            this.elevatorWhoRisedAnEvent = elevatorWhoRisedAnEvent;
        }
    }
}
