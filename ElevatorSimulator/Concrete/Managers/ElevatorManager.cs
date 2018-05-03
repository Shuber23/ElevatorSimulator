using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    internal class ElevatorManager: Manager, IMovable
    {
        private readonly List<Elevator> elevators;
        private object locker = new object();

        public ElevatorManager(IDispatcher dispatcher, List<Elevator> elevators) : base(dispatcher) => this.elevators = elevators;

        public void SendElevatorForPassenger(Passenger passenger)
        {
            Elevator elevator = FindAvaliableElevator(passenger.Direction);
            Console.WriteLine("Elevator number {0} founded!", elevator.elevatorIndex);
            lock (locker)
            {
                elevator.DestinationFloorIndexes.Add(passenger.CurrentFloorIndex);
            }

            //Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
            UpdateElevatorDirection(elevator);
            TryMove(elevator);
            TryEnterElevator(elevator, passenger);
            OnPassengerEnteredElevator(new PassengerEventArgs(passenger));
            lock (locker)
            {
                elevator.DestinationFloorIndexes.Remove(passenger.CurrentFloorIndex);
            }
            Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
            TryMove(elevator);
            //ExitElevator(elevator, passenger);
            //elevator.DestinationFloorIndexes.Add(0);
            Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
            //TryMove(elevator);
            Console.WriteLine("Elevator {0} end work!", elevator.elevatorIndex);
            Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
        }

        private List<Elevator> GetElevatorsByStatus(States.ElevatorState state)
        {
            //lock (locker)
            //{
             return elevators.FindAll(x => x.state == state);
            //}
        }

        private List<Elevator> GetElevatorsByCurrentFloorPosition(int floorIndex) =>
            elevators.FindAll(x => x.CurrentFloorIndex == floorIndex);

        public override object GetItem(States.Direction passengerDirection) =>
            FindAvaliableElevator(passengerDirection);

        private Elevator FindAvaliableElevator(States.Direction passengerDirection)
        {
            while (true)
            {
                var elevatorWaitingList = GetElevatorsByStatus(States.ElevatorState.Waiting);
                if (elevatorWaitingList.Count > 0)
                {
                    return elevatorWaitingList.First();
                }

                switch (passengerDirection)
                {
                    case States.Direction.Up:
                        var elevatorByGoingUpList = GetElevatorsByStatus(States.ElevatorState.GoingUp);
                        if (elevatorByGoingUpList.Count > 0)
                        {
                            return elevatorByGoingUpList.First();
                        }
                        break;
                    case States.Direction.Down:
                        var elevatorByGoingDownUpList = GetElevatorsByStatus(States.ElevatorState.GoingDown);
                        if (elevatorByGoingDownUpList.Count > 0)
                        {
                            return elevatorByGoingDownUpList.First();
                        }
                        break;
                 }
            }

        }

        private void TryEnterElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                if (!elevator.CanUseElevator(passenger.Weight))
                {
                    Console.WriteLine("Elevator {0} is full for passenger {1}!", elevator.elevatorIndex, passenger.passengerIndex);
                    elevator.DestinationFloorIndexes.Remove(passenger.CurrentFloorIndex);
                    OnPassengerCalledElevator(new PassengerEventArgs(passenger));
                    return;
                }

                elevator.EnterInElevator(passenger);
                elevator.DestinationFloorIndexes.Remove(passenger.CurrentFloorIndex);
                elevator.DestinationFloorIndexes.Add(passenger.DestinationFloorIndex);
                Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
                Console.WriteLine("Passenger {0} entered elevator!", passenger.passengerIndex);
                UpdateElevatorDirection(elevator);
                System.Threading.Thread.Sleep(1000);
            }
        }

        private void ExitElevator(Elevator elevator, Passenger passenger)
        {
            lock (locker)
            {
                elevator.ExitFromElevator(passenger);
                elevator.DestinationFloorIndexes.Remove(passenger.DestinationFloorIndex);
                Print(elevator.DestinationFloorIndexes, elevator.elevatorIndex);
                Console.WriteLine("Passenger {0} exited elevator!", passenger.passengerIndex);
                UpdateElevatorDirection(elevator);
            }
        }

        private int GetClosestDestinationPointIndex(Elevator elevator)
        {
            var indexes = elevator.DestinationFloorIndexes;
            for (int i = 0; i < indexes.Count; i++)
            {
                indexes[i] = Math.Abs(indexes[i] - elevator.CurrentFloorIndex);
            }
            return indexes.Min();
        }

        private void UpdateElevatorDirection(Elevator elevator)
        {
            if (elevator.DestinationFloorIndexes.Count <= 0)
            {
                elevator.state = States.ElevatorState.Waiting;
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
            Console.WriteLine("Elevator {0} direction updated!", elevator.elevatorIndex);
        }

        private void ReleaseAllArrivedPassengerIfNeeded(Elevator elevator)
        {
            lock (locker)
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
                }
            }
            System.Threading.Thread.Sleep(1000);
        }

        private void TryMove(Elevator elevator)
        {
            bool isElevatorEmpty = false;
            Console.WriteLine("Elevator {0} began move!", elevator.elevatorIndex);
            if (elevator.state == States.ElevatorState.GoingUp)
            {
                lock (locker)
                {

                    while (!isElevatorEmpty && elevator.CurrentFloorIndex < elevator.DestinationFloorIndexes.First())
                    {
                        Console.WriteLine("Elevator {0} on floor {1}!", elevator.elevatorIndex,
                            elevator.CurrentFloorIndex);
                        System.Threading.Thread.Sleep(5000);
                        elevator.CurrentFloorIndex++;
                        ReleaseAllArrivedPassengerIfNeeded(elevator);
                        isElevatorEmpty = elevator.IsEmpty;
                    }
                }
                return;
            }
            if (elevator.state == States.ElevatorState.GoingDown)
            {
                lock (locker)
                {
                    while (!isElevatorEmpty && elevator.CurrentFloorIndex > elevator.DestinationFloorIndexes.First())
                    {
                        Console.WriteLine("Elevator {0} on floor {1}!", elevator.elevatorIndex,
                            elevator.CurrentFloorIndex);
                        System.Threading.Thread.Sleep(3000);
                        elevator.CurrentFloorIndex--;
                        ReleaseAllArrivedPassengerIfNeeded(elevator);
                        isElevatorEmpty = elevator.IsEmpty;
                    }
                }
            }
        }

        private void Print(List<int> list, int elevatorIndex)
        {
            lock (locker)
            {
                Console.Write("Elevator " + elevatorIndex + ":");
                foreach (var index in list)
                {
                    Console.Write("   " + index + " ");
                }
                Console.WriteLine();
            }
        }

    }
}
