using System;
using System.IO.Ports;
using System.Text;

namespace SerialPortTest.io
{
    public class SerialPortReal : ISerialPort
    {
        private const int MAX_CAPACITY = 10 * 1024;

        private bool disposed = false;

        private readonly object bufferCriticalSection = new object();
        private readonly StringBuilder receivedDataBuffer = new StringBuilder(MAX_CAPACITY);

        private readonly SerialPort serialPort;


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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                serialPort.DataReceived -= SerialPort_DataReceived;
                serialPort.Dispose();
            }

            disposed = true;
        }
    }
}