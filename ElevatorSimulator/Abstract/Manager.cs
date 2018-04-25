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

        protected Manager(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        public abstract T GetItem<T>() where T : class;
    }
}
