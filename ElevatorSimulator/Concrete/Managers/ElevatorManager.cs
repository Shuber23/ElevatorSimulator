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
        private object locker = new object();

        public ElevatorManager(IDispatcher dispatcher, List<Elevator> elevators) : base(dispatcher)
        {
            this.elevators = elevators;
            TimerCallback timerCallback = CheckingIfPassengerNeedsElevatorCallBack;
            Timer timer = new Timer(timerCallback, null, 0, 1000);
        }

        public void SendElevator(Elevator elevator, Passenger passenger)
        {
            Console.WriteLine("Elevator number {0} founded!", elevator.elevatorIndex);
            AddDestinationFloorIndexesList(elevator, passenger.CurrentFloorIndex);
            UpdateElevatorDirection(elevator);
            TryMove(elevator);
            TryEnterElevator(elevator, passenger);
            RemoveDestinationFloorIndexesList(elevator, passenger.CurrentFloorIndex);
            TryMove(elevator);
            UpdateElevatorDirection(elevator);
            Console.WriteLine("Elevator {0} end work! Status: {1}", elevator.elevatorIndex, elevator.state);
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
                SendElevator(elevatorsList.First(), waitingPassenger);
            }
        }

        private void TryEnterElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                if (!elevator.CanUseElevator(passenger.Weight))
                {
                    Console.WriteLine("Elevator {0} is full for passenger {1}!", elevator.elevatorIndex, passenger.passengerIndex);
                    return;
                }

                elevator.EnterInElevator(passenger);
                RemoveDestinationFloorIndexesList(elevator, passenger.CurrentFloorIndex);
                AddDestinationFloorIndexesList(elevator, passenger.DestinationFloorIndex);
                Console.WriteLine("Passenger {0} entered elevator!", passenger.passengerIndex);
                UpdateElevatorDirection(elevator);
                OnPassengerEnteredElevator(new PassengerEventArgs(passenger));
            }
        }

        private void ExitElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                elevator.ExitFromElevator(passenger);
                RemoveDestinationFloorIndexesList(elevator, passenger.DestinationFloorIndex);
                Console.WriteLine("Passenger {0} released elevator!", passenger.passengerIndex);
                UpdateElevatorDirection(elevator);
            }
        }

        private void UpdateElevatorDirection(Elevator elevator)
        {
            if (elevator.DestinationFloorIndexes.Count <= 0)
            {
                elevator.state = States.ElevatorState.Waiting;
                OnElevatorUpdatedDirection(new ElevatorEventArgs(elevator));
                return;
            }
            if (elevator.CurrentFloorIndex < elevator.DestinationFloorIndexes.First())
            {
                elevator.state = States.ElevatorState.GoingUp;
            }
            else
            {
                elevator.state = States.ElevatorState.GoingDown;
            }
            OnElevatorUpdatedDirection(new ElevatorEventArgs(elevator));
            Console.WriteLine("Elevator {0} direction updated!", elevator.elevatorIndex);
        }

        private void ReleaseAllArrivedPassengerIfNeeded(Elevator elevator)
        {
            List<Passenger> releasedPassenger = new List<Passenger>();
            foreach (var passenger in elevator.GetPeopleInsideList())
            {
                if (passenger.DestinationFloorIndex == elevator.CurrentFloorIndex)
                {
                    releasedPassenger.Add(passenger);
                }
            }
            foreach (var passenger in releasedPassenger)
            {
                ExitElevator(elevator, passenger);
                OnPassengerReleasedElevator(new PassengerEventArgs(passenger));
            }
        }

        private void TryMove(Elevator elevator)
        {
            int destinationIndex;
            Console.WriteLine("Elevator {0} began move!", elevator.elevatorIndex);
            if (elevator.state == States.ElevatorState.GoingUp)
            {
                lock (locker)
                {
                    destinationIndex = elevator.DestinationFloorIndexes.First();
                }

                while (elevator.CurrentFloorIndex < destinationIndex)
                {
                    elevator.CurrentFloorIndex++;
                    Move(elevator);
                }
                return;
            }
            if (elevator.state == States.ElevatorState.GoingDown)
            {
                lock (locker)
                {
                    destinationIndex = elevator.DestinationFloorIndexes.First();
                }
                while (elevator.CurrentFloorIndex > destinationIndex)
                {
                    elevator.CurrentFloorIndex--;
                    Move(elevator);
                }
            }
        }

        private void Move(Elevator elevator)
        {
            lock (locker)
            {
                Console.WriteLine("Elevator {0} on floor {1}!", elevator.elevatorIndex,
                    elevator.CurrentFloorIndex);
                Console.WriteLine("People inside in elevator {0}: {1}", elevator.elevatorIndex,
                    elevator.GetPeopleInsideList().Count);
                Thread.Sleep(5000);
                ReleaseAllArrivedPassengerIfNeeded(elevator);
                EnterWaitingPassengerInFloorIfPossible(elevator);
            }
        }

        private void EnterWaitingPassengerInFloorIfPossible(Elevator elevator)
        {
            List<Passenger> waitingPassengers = new List<Passenger>();
            if (elevator.state == States.ElevatorState.GoingUp)
            {
                waitingPassengers =
                    GetAllPassengersOnCurrentFloorByDirection(elevator.CurrentFloorIndex, States.Direction.Up).ToList();
                Console.WriteLine("Waiting passenger count on floor {0}: {1}", elevator.CurrentFloorIndex, waitingPassengers.Count);
            }
            if (elevator.state == States.ElevatorState.GoingDown)
            {
                waitingPassengers =
                    GetAllPassengersOnCurrentFloorByDirection(elevator.CurrentFloorIndex, States.Direction.Down).ToList();
                Console.WriteLine("Waiting passenger count on floor {0}: {1}", elevator.CurrentFloorIndex, waitingPassengers.Count);
            }
            foreach (var passenger in waitingPassengers)
            {
                TryEnterElevator(elevator, passenger);
            }
        }

        private List<Passenger> GetAllPassengersOnCurrentFloorByDirection(int floorIndex, States.Direction direction)
        {
            lock (locker)
            {
                return ((IQueue)dispatcher.QueueManager).GetAllPassengersOnFloorByDirection(floorIndex, direction);
            }
        }

        private void AddDestinationFloorIndexesList(Elevator elevator, int floorIndex)
        {
            elevator.DestinationFloorIndexes.Add(floorIndex);
        }

        private void RemoveDestinationFloorIndexesList(Elevator elevator, int floorIndex)
        {
            elevator.DestinationFloorIndexes.Remove(floorIndex);
        }

        public override void Create()
        {
            throw new NotImplementedException();
        }
    }
}
