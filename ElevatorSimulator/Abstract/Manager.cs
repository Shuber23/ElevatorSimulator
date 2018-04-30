using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Abstract
{
    public abstract class Manager
    {
        protected readonly IDispatcher dispatcher;

        public event EventHandler passengerCalledElevator;

        protected Manager(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        public abstract object GetItem(States.Direction direction);

        public void OnPassengerCalledElevator(EventArgs e)
        {
            passengerCalledElevator?.Invoke(this, e);
        }
    }
}
