using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Abstract
{
    internal abstract class Button
    {
        public event EventHandler ButtonPressed;
        public int floorLocation;
        protected Button(int floorLocation)
        {
            this.floorLocation = floorLocation;
        }

        //public void Press()
        //{
        //    ButtonPressed += Dispatcher.GetInstance().OnButtonPressed;
        //}

    }
}
