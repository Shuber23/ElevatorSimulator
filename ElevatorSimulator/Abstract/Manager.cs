using System;
using System.Collections.Generic;
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
        public event PassengerEventHandler PassengerCalledElevator;
        public event PassengerEventHandler PassengerEnteredElevator;

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

        protected Manager(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        public abstract object GetItem(States.Direction direction);
    }
}
