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

        public ElevatorManager(IDispatcher dispatcher, List<Elevator> elevators) : base(dispatcher) => this.elevators = elevators;

        public void SendElevatorForPassenger(Passenger passenger)
        {
            Elevator elevator = FindAvaliableElevator(passenger.Direction);
            elevator.DestinationFloorIndexes.Add(passenger.CurrentFloorIndex);
            UpdateElevatorDirection(elevator);
            TryMove(elevator);
            TryEnterElevator(elevator, passenger);
            OnPassengerEnteredElevator(new PassengerEventArgs(passenger));
            elevator.DestinationFloorIndexes.Remove(passenger.CurrentFloorIndex);
            TryMove(elevator);
            elevator.DestinationFloorIndexes.Add(0);
            TryMove(elevator);
        }

        private List<Elevator> GetElevatorsByStatus(States.ElevatorState state) =>
            elevators.FindAll(x => x.state == state);

        private List<Elevator> GetElevatorsByCurrentFloorPosition(int floorIndex) =>
            elevators.FindAll(x => x.CurrentFloorIndex == floorIndex);

        public override object GetItem(States.Direction passengerDirection) =>
            FindAvaliableElevator(passengerDirection);

        private Elevator FindAvaliableElevator(States.Direction passengerDirection)
        {
            if (GetElevatorsByStatus(States.ElevatorState.Waiting).First() != null)
            {
                return GetElevatorsByStatus(States.ElevatorState.Waiting).First();
            }

            switch (passengerDirection)
            {
                case States.Direction.Up:
                    return GetElevatorsByStatus(States.ElevatorState.GoingUp).First();
                case States.Direction.Down:
                    return GetElevatorsByStatus(States.ElevatorState.GoingDown).First();
                default:
                    return null;
            }
        }

        private void TryEnterElevator(Elevator elevator, Passenger passenger)
        {
            if (elevator.IsFull)
            {
                return;
            }
            elevator.EnterInElevator(passenger);
            elevator.DestinationFloorIndexes.Remove(passenger.CurrentFloorIndex);
            elevator.DestinationFloorIndexes.Add(passenger.DestinationFloorIndex);
            UpdateElevatorDirection(elevator);
        }

        private void ExitElevator(Elevator elevator, Passenger passenger)
        {
            elevator.ExitFromElevator(passenger);
            elevator.DestinationFloorIndexes.Remove(passenger.DestinationFloorIndex);
            UpdateElevatorDirection(elevator);
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
            //int destinationIndex = GetClosestDestinationPointIndex(elevator);
            if (elevator.CurrentFloorIndex < elevator.DestinationFloorIndexes.First())
            {
                elevator.state = States.ElevatorState.GoingUp;
            }
            else
            {
                elevator.state = States.ElevatorState.GoingDown;
            }
        }

        private List<Passenger> GetAllArrivedPassengerOnFloor(Elevator elevator)
        {
            List<Passenger> passengersNeededExit = new List<Passenger>();
            foreach (var passenger in elevator.GetPeopleInsideList())
            {
                if (passenger.DestinationFloorIndex == elevator.CurrentFloorIndex)
                {
                    passengersNeededExit.Add(passenger);
                    elevator.ExitFromElevator(passenger);
                }
            }
            return passengersNeededExit;
        }

        private void TryMove(Elevator elevator)
        {
            if (elevator.state == States.ElevatorState.GoingUp)
            {
                while (elevator.CurrentFloorIndex < elevator.DestinationFloorIndexes.First())
                {
                    System.Threading.Thread.Sleep(5000);
                    elevator.CurrentFloorIndex++;
                }
                return;
            }
            if (elevator.state == States.ElevatorState.GoingDown)
            {
                while (elevator.CurrentFloorIndex > elevator.DestinationFloorIndexes.First())
                {
                    System.Threading.Thread.Sleep(5000);
                    elevator.CurrentFloorIndex--;
                }
            }
        }

    }
}
