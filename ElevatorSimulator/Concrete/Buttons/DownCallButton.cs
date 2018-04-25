using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Abstract;

namespace ElevatorSimulator.Concrete.Buttons
{
    internal class DownCallButton: Button
    {
        public DownCallButton(int floorLocation): base(floorLocation)
        {

        }
    }
}
