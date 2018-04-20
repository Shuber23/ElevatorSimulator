using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    public class States
    {
        public enum ElevatorState
        {
            Waiting,
            Preparing,
            GoingUp,
            GoingDown
        }

        public enum PassengerState
        {
            Waiting,
            MovingInElevator,
            Inside,
            Leaving
        }

        public enum Direction
        {
            Up,
            Down,
            None
        }
    }
}
