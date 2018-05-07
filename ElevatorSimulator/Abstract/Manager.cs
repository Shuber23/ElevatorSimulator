using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Events;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Abstract
{

    public delegate void PassengerEventHandler(object sender, PassengerEventArgs e);
    public delegate void ElevatorEventHandler(object sender, ElevatorEventArgs e);

    public abstract class Manager
    {
        protected readonly IDispatcher dispatcher;


        public event ElevatorEventHandler ElevatorArrived;
        public event ElevatorEventHandler ElevatorUpdatedDirection;
        public event PassengerEventHandler PassengerCalledElevator;
        public event PassengerEventHandler PassengerEnteredElevator;
        public event PassengerEventHandler PassengerReleasedElevator;

        protected Manager(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        public abstract void Create();

        protected void OnElevatorUpdatedDirection(ElevatorEventArgs e)
        {
            ElevatorUpdatedDirection?.Invoke(this, e);
        }

        protected void OnElevatorArrived(ElevatorEventArgs e)
        {
            ElevatorArrived?.Invoke(this, e);
        }

        protected void OnPassengerCalledElevator(PassengerEventArgs e)
        {
            PassengerCalledElevator?.Invoke(this, e);
        }

        protected void OnPassengerEnteredElevator(PassengerEventArgs e)
        {
            PassengerEnteredElevator?.Invoke(this, e);
        }

        protected void OnPassengerReleasedElevator(PassengerEventArgs e)
        {
            PassengerReleasedElevator?.Invoke(this, e);
        }
    }
}
