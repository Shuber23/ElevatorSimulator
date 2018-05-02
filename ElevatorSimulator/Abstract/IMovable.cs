using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Abstract
{
    public interface IMovable
    {
        void SendElevatorForPassenger(Passenger passenger);
    }
}
