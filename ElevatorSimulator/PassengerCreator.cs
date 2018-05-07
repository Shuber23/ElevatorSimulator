using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator
{
    class PassengerCreator
    {
        private TimerCallback timerCallback;
        private Timer timer;
        private void Create(object obj)
        {
            int number = new Random().Next(1, 101);
            if (number > 51)
            {
                Dispatcher.GetInstance().PassengerManager.Create();
            }
        }

        public void StartPassengerGenerator()
        {
            timerCallback = Create;
            timer = new Timer(timerCallback, null, 0, 5000);
        }

    }
}
