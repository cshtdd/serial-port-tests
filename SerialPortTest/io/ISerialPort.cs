using System;

namespace SerialPortTest.io
{
    public interface ISerialPort : IDisposable
    {
        void Open();
        void Close();
        void ConfigureReceiver(Action<String> receiver);
        void Send(string data);
    }
}
