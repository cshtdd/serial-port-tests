using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace SerialPortTest.io
{
    public class SerialPortReal : ISerialPort
    {
        private const int MaxCapacity = 10 * 1024;

        private bool _disposed;

        private Action<string> _receiver;
        private readonly object _bufferCriticalSection = new object();
        private readonly StringBuilder _receivedDataBuffer = new StringBuilder(MaxCapacity);

        private readonly SerialPort _serialPort;
        private readonly Timer _receiverTimer;


        public SerialPortReal(string port)
        {
            _serialPort = new SerialPort(port);
            _serialPort.DataReceived += SerialPort_DataReceived;
            _receiverTimer = new Timer(ReceiverTimer_Elapsed, null, 250, 250);
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

                _receiverTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _receiverTimer.Dispose();
            }

            _disposed = true;
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var readData = _serialPort.ReadExisting();
            AppendToBuffer(readData);
        }

        private void ReceiverTimer_Elapsed(object state)
        {
            var data = ReadBufferData();

            if (!string.IsNullOrEmpty(data))
            {
                InvokeReceiver(data);
            }
        }       

        public void Open()
        {
            _serialPort.Open();
        }

        public void Close()
        {
            _serialPort.Close();
        }

        public void ConfigureReceiver(Action<string> receiver)
        {
            _receiver = receiver;
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

        private string ReadBufferData()
        {
            lock (_bufferCriticalSection)
            {
                var result = _receivedDataBuffer.ToString();
                _receivedDataBuffer.Clear();
                return result;
            }
        }

        private void InvokeReceiver(string data)
        {
            _receiver?.Invoke(data);
        }
    }
}