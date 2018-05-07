using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator
{
    class ManagerInitializator
    {
        private IDispatcher dispatcher;

        internal void InitializeDispatcher()
        {
            dispatcher = Dispatcher.GetInstance();
        }

        internal void InitializeManagers()
        {

        }
    }
}
