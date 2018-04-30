using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    internal class ElevatorManager: Manager
    {
        private readonly List<Elevator> elevators;

        public event EventHandler PassengerEnteredElevator;

        public ElevatorManager(IDispatcher dispatcher, List<Elevator> elevators) : base(dispatcher) => this.elevators = elevators;

        private List<Elevator> GetElevatorsByStatus(States.ElevatorState state) =>
            elevators.FindAll(x => x.state == state);

        private List<Elevator> GetElevatorsByCurrentFloorPosition(int floorIndex) =>
            elevators.FindAll(x => x.CurrentFloorIndex == floorIndex);

        public override object GetItem(States.Direction passengerDirection) =>
            FindAvaliableElevator(passengerDirection);

        private Elevator FindFreeElevator() => 
            GetElevatorsByStatus(States.ElevatorState.Waiting).First();

        private Elevator FindAvaliableElevator(States.Direction passengerDirection)
        {
            Elevator elevator = FindFreeElevator();
            if (elevator != null)
            {
                return elevator;
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



    }
}
