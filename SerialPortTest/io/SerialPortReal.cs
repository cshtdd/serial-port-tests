using System;
using System.IO.Ports;
using System.Text;

namespace SerialPortTest.io
{
    public class SerialPortReal : ISerialPort
    {
        private const int MAX_CAPACITY = 10 * 1024;
        private readonly SerialPort serialPort;

        private readonly object bufferCriticalSection = new object();
        private readonly StringBuilder receivedDataBuffer = new StringBuilder(MAX_CAPACITY);

        public SerialPortReal(string port)
        {
            serialPort = new SerialPort(port);
            serialPort.DataReceived += SerialPort_DataReceived;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //serialPort.ReadExisting()
        }

        public void Send(byte[] data)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}