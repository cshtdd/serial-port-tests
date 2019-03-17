using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTest.io
{
    interface ISerialPort
    {
        void Send(byte[] data);
    }
}
