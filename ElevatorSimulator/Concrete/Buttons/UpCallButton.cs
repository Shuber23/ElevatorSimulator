using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;
using ElevatorSimulator.Concrete.Managers;

namespace ElevatorSimulator.Concrete.Buttons
{
    internal class UpCallButton: Button
    {
        public UpCallButton(int floorLocation) : base(floorLocation)
        {
            
        }
    }
}
