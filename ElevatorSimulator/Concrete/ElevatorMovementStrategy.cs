using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete
{
    class ElevatorMovementStrategy: IElevatorMovementStrategyStrategy
    {
        private object locker = new object();
        private IDispatcher dispatcher;

        public ElevatorMovementStrategy(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void ElevatorMovementAlgorhitm(Elevator elevator, Passenger passenger)
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
                Console.WriteLine("Passenger {0} entered elevator {1}!", passenger.passengerIndex, elevator.elevatorIndex);
                UpdateElevatorDirection(elevator);
                GlobalEvents.OnPassengerEnteredElevator(new PassengerEventArgs(passenger));
                Console.WriteLine("Passenger {0} removed from queue!", passenger.passengerIndex);
            }
        }

        private void ExitElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                elevator.ExitFromElevator(passenger);
                RemoveDestinationFloorIndexesList(elevator, passenger.DestinationFloorIndex);
                Console.WriteLine("Passenger {0} released elevator {1}!", passenger.passengerIndex, elevator.elevatorIndex);
                GlobalEvents.OnPassengerReleasedElevator(new PassengerEventArgs(passenger));
                UpdateElevatorDirection(elevator);
            }
        }

        private void UpdateElevatorDirection(Elevator elevator)
        {
            if (elevator.DestinationFloorIndexes.Count <= 0)
            {
                elevator.state = States.ElevatorState.Waiting;
                GlobalEvents.OnElevatorUpdatedDirection(new ElevatorEventArgs(elevator));
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
            GlobalEvents.OnElevatorUpdatedDirection(new ElevatorEventArgs(elevator));
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
                GlobalEvents.OnPassengerReleasedElevator(new PassengerEventArgs(passenger));
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
            }
            if (elevator.state == States.ElevatorState.GoingDown)
            {
                waitingPassengers =
                    GetAllPassengersOnCurrentFloorByDirection(elevator.CurrentFloorIndex, States.Direction.Down).ToList();
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
    }
}
