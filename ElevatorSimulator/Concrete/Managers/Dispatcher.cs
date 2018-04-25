using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Concrete.Managers
{
    class Dispatcher: IDispatcher
    {
        private static Dispatcher instance;
        public Manager QueueManager { get; set; }
        public Manager ElevatorManager { get; set; }
        public Manager PassengerManager { get; set; }
        
        //private readonly List<Elevator> elevators;
        private Timer floorChecker;

        private readonly object locker = new object();

        public static Dispatcher GetInstance(List<Floor> floors = null)
        {
            if (instance == null)
            {
                instance = new Dispatcher();
            }
            return instance;
        }

        public Elevator FindAvaliableElevator(States.Direction direction) => ElevatorManager.GetItem<Elevator>();

        public void OnButtonPressed(object sender, EventArgs e)
        {
           //queueManager.AddToQueue();
        }
    }
}
