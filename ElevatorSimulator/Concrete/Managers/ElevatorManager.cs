using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    internal class ElevatorManager: Manager
    {
        private readonly List<Elevator> elevators;
        private TimerCallback timerCallback;
        private Timer timer;
        private object locker = new object();

        private readonly IElevatorMovementStrategyStrategy strategy;

        public ElevatorManager(IDispatcher dispatcher, IElevatorMovementStrategyStrategy strategy, List<Elevator> elevators) : base(dispatcher)
        {
            this.elevators = elevators;
            this.strategy = strategy;
            timerCallback = CheckingIfPassengerNeedsElevatorCallBack;
            timer = new Timer(timerCallback, null, 0, 1000);
        }

        private List<Elevator> GetElevatorsByStatus(States.ElevatorState state)
        {
            return elevators.FindAll(x => x.state == state);
        }

        private void CheckingIfPassengerNeedsElevatorCallBack(object obj)
        {
            Passenger waitingPassenger = ((IQueue)dispatcher.QueueManager).GetWaitingPassenger();
            List<Elevator> elevatorsList = GetElevatorsByStatus(States.ElevatorState.Waiting);
            if (elevatorsList.Any() && waitingPassenger != null)
            {
                strategy.ElevatorMovementAlgorhitm(elevatorsList.First(), waitingPassenger);
            }
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
