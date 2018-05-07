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
    public abstract class Manager
    {
        protected readonly IDispatcher dispatcher;

        protected Manager(IDispatcher dispatcher) => this.dispatcher = dispatcher;

        public abstract void Create();
    }
}
