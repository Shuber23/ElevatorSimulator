using System;
using System.Collections.Concurrent;
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
        private FileLogger logger = new FileLogger();
        private bool wasCalled;

        public ElevatorMovementStrategy(IDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public void ElevatorMovementAlgorhitm(Elevator elevator, Passenger passenger)
        {
            wasCalled = true;
            logger.Write("Elevator number " + elevator.elevatorIndex + " founded!", elevator.elevatorIndex);
            AddDestinationFloorIndexesConcurrentBag(elevator, passenger.CurrentFloorIndex);
            UpdateElevatorDirection(elevator);
            TryMove(elevator);
            TryEnterElevator(elevator, passenger);
            TryMove(elevator);
            UpdateElevatorDirection(elevator);
            logger.Write("Elevator" + elevator.elevatorIndex + " end work! Status: " + elevator.state,
                elevator.elevatorIndex);
        }

        private void TryEnterElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                if (!elevator.CanUseElevator(passenger.Weight))
                {
                    Console.WriteLine("Elevator " + elevator.elevatorIndex + " is full for passenger " + passenger.passengerIndex,
                        elevator.elevatorIndex);
                    logger.Write(
                        "Elevator " + elevator.elevatorIndex + " is full for passenger " + passenger.passengerIndex,
                        elevator.elevatorIndex);
                    GlobalEvents.OnPassengerCalledElevator(new PassengerEventArgs(passenger));
                    return;
                }

                elevator.EnterInElevator(passenger);
                if (wasCalled)
                {
                    RemoveDestinationFloorIndexesConcurrentBag(elevator, passenger.CurrentFloorIndex);
                    wasCalled = false;
                }

                AddDestinationFloorIndexesConcurrentBag(elevator, passenger.DestinationFloorIndex);
                logger.Write("Passenger "+ passenger.passengerIndex+ " entered elevator" + elevator.elevatorIndex, elevator.elevatorIndex);
                Console.WriteLine("Passengers inside in elevator {0}: {1}, total weight: {2}", elevator.elevatorIndex, elevator.GetPeopleInside().Count, elevator.WeightInside);
                UpdateElevatorDirection(elevator);
                GlobalEvents.OnPassengerEnteredElevator(new PassengerEventArgs(passenger));
            }
        }

        private void ExitElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                elevator.ExitFromElevator(passenger);
                RemoveDestinationFloorIndexesConcurrentBag(elevator, passenger.DestinationFloorIndex);
                logger.Write("Passenger " + passenger.passengerIndex + " released elevator" + elevator.elevatorIndex, elevator.elevatorIndex);
                GlobalEvents.OnPassengerReleasedElevator(new PassengerEventArgs(passenger));
            }
        }

        private void UpdateElevatorDirection(Elevator elevator)
        {
            if (elevator.IsEmpty && elevator.DestinationFloorIndexes.Count <= 0)
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
            logger.Write("Elevator " + elevator.elevatorIndex + " direction updated!", elevator.elevatorIndex);
        }

        private void ReleaseAllArrivedPassengerIfNeeded(Elevator elevator)
        {
            List<Passenger> releasedPassenger = new List<Passenger>();
            foreach (var passenger in elevator.GetPeopleInside())
            {
                if (passenger.DestinationFloorIndex == elevator.CurrentFloorIndex)
                {
                    releasedPassenger.Add(passenger);
                }
            }
            foreach (var passenger in releasedPassenger)
            {
                ExitElevator(elevator, passenger);
                Thread.Sleep(500);
            }
            UpdateElevatorDirection(elevator);
        }

        private void TryMove(Elevator elevator)
        {
            int destinationIndex;
            logger.Write("Elevator " + elevator.elevatorIndex + " began move!", elevator.elevatorIndex);
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
                logger.Write("Elevator " + elevator.elevatorIndex + " on floor" + elevator.CurrentFloorIndex,
                    elevator.elevatorIndex);
                Thread.Sleep(5000);
                ReleaseAllArrivedPassengerIfNeeded(elevator);
                if (!elevator.IsFull)
                {
                    EnterWaitingPassengerInFloorIfPossible(elevator);
                }
            }
        }

        private void EnterWaitingPassengerInFloorIfPossible(Elevator elevator)
        {
            ConcurrentBag<Passenger> waitingPassengers = new ConcurrentBag<Passenger>();
            if (elevator.state == States.ElevatorState.GoingUp)
            {
                waitingPassengers =
                    GetAllPassengersOnCurrentFloorByDirection(elevator.CurrentFloorIndex, States.Direction.Up);
            }

            if (elevator.state == States.ElevatorState.GoingDown)
            {
                waitingPassengers =
                    GetAllPassengersOnCurrentFloorByDirection(elevator.CurrentFloorIndex, States.Direction.Down);
            }

            foreach (var passenger in waitingPassengers)
            {
                TryEnterElevator(elevator, passenger);
                Thread.Sleep(500);
            }
        }

        private ConcurrentBag<Passenger> GetAllPassengersOnCurrentFloorByDirection(int floorIndex, States.Direction direction)
        {
            lock (locker)
            {
                return ((IQueue)dispatcher.QueueManager).GetAllPassengersOnFloorByDirection(floorIndex, direction);
            }
        }

        private void AddDestinationFloorIndexesConcurrentBag(Elevator elevator, int floorIndex)
        {
            elevator.DestinationFloorIndexes.Add(floorIndex);
            Console.WriteLine("Destination point for elevator {0} added, thread: {1}", elevator.elevatorIndex,
                Thread.CurrentThread.ManagedThreadId);
            //Print(elevator);
        }

        private void RemoveDestinationFloorIndexesConcurrentBag(Elevator elevator, int floorIndex)
        {
            elevator.DestinationFloorIndexes.TryTake(out floorIndex);
            Console.WriteLine("Destination point for elevator {0} removed, thread: {1}", elevator.elevatorIndex, Thread.CurrentThread.ManagedThreadId);
            //Print(elevator);
        }

        private void Print(Elevator elevator)
        {
            Console.Write("Destination floor indexes for elevator {0}:   ", elevator.elevatorIndex);
            foreach (var index in elevator.DestinationFloorIndexes)
            {
                Console.Write(" "+ index+ " ");
            }
            Console.WriteLine();
        }
    }
}
