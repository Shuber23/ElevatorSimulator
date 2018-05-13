using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Abstract
{
    interface IQueue
    {
        void AddToQueue(Passenger passenger);
        void RemoveFromQueue(Passenger passenger);
        Passenger GetWaitingPassenger();
        ConcurrentBag<Passenger> GetAllPassengersOnFloorByDirection(int floorIndex, States.Direction direction);
        List<ConcurrentBag<Passenger>> GetAllPassengers();
    }
}
