﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElevatorSimulator.Models;

namespace ElevatorSimulator.Abstract
{ 
    interface IElevatorMovementStrategyStrategy
    {
        void ElevatorMovementAlgorhitm(Elevator elevator, Passenger passenger);
    }
}