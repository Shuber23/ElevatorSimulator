using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete
{
    class PassengerGenerator: IGenerator
    {
        public Passenger Generate()
        {
            Randomizer.Random();
            return new Passenger(Randomizer.weight, States.Direction.None,
                Randomizer.currentFloor, Randomizer.destinationFloor, Randomizer.passengerIndex);
        }
    }
}
