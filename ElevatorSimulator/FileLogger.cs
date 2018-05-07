using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevatorSimulator
{
    class FileLogger
    {
        public void Write(string message, int elevatorIndex)
        {
            string path = "C:\\Users\\tshub\\Desktop\\Elevator_" + elevatorIndex + ".txt";

            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(message);
            }
        }
    }
}
