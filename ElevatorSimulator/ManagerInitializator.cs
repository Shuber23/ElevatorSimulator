using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete;
using ElevatorSimulator.Concrete.Managers;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator
{
    class ManagerInitializator
    {
        private IDispatcher dispatcher;

        internal void InitializeDispatcher()
        {
            dispatcher = Dispatcher.GetInstance();
        }

        internal void InitializeManagers()
        {
            Manager passengerManager = new PassengerManager(dispatcher, new PassengerGenerator());
            Manager elevatorManager = new ElevatorManager(dispatcher, new ElevatorMovementStrategy(dispatcher),  Building.GetInstance().Elevators);
            Manager queueManager = new QueueManager(dispatcher, Building.GetInstance().Floors);

            dispatcher.PassengerManager = passengerManager;
            dispatcher.ElevatorManager = elevatorManager;
            dispatcher.QueueManager = queueManager;
        }

        internal void SubscribeOnEvents()
        {
            GlobalEvents.PassengerCalledElevator += dispatcher.PassengerCalledElevatorEventHandler;
            GlobalEvents.PassengerEnteredElevator += dispatcher.PassengerEnteredElevatorEventHandler;
            GlobalEvents.PassengerReleasedElevator += dispatcher.PassengerReleasedElevatorEventHandler;
        }
    }
}
