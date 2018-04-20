using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator.Abstract
{
    abstract class Button
    {
        protected event EventHandler OnButtonPressed;
        protected bool wasPressed;

    }
}
