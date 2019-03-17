using System;
using System.IO.Ports;
using System.Text;

namespace SerialPortTest.io
{
    public class SerialPortReal : ISerialPort
    {
        private const int MAX_CAPACITY = 10 * 1024;

        private bool _disposed = false;

        private readonly object _bufferCriticalSection = new object();
        private readonly StringBuilder _receivedDataBuffer = new StringBuilder(MAX_CAPACITY);

        private readonly SerialPort _serialPort;


        public SerialPortReal(string port)
        {
            _serialPort = new SerialPort(port);
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _serialPort.DataReceived -= SerialPort_DataReceived;
                _serialPort.Dispose();
            }

            _disposed = true;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var readData = _serialPort.ReadExisting();
            AppendToBuffer(readData);
        }

        public void Send(string data)
        {
            _serialPort.Write(data);
        }

        private void AppendToBuffer(string data)
        {
            lock (_bufferCriticalSection)
            {
                _receivedDataBuffer.Append(data);
            }
        }
    }
}