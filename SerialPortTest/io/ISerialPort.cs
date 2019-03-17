using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerialPortTest.io
{
    public interface ISerialPort : IDisposable
    {
        void Send(string data);
    }
}
