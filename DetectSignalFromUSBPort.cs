using System;
using System.IO.Ports;

namespace Redscreen;

public class DetectSignalFromUSBPort : IDisposable
{
    private readonly SerialPort serialPort;

    private bool disposed;

    public event Action? SuccessReceived;

    public DetectSignalFromUSBPort(
        string portName,
        int baudRate)
    {
        serialPort =
            new SerialPort(
                portName,
                baudRate);

        serialPort.NewLine = "\n";

        serialPort.DataReceived += OnDataReceived;
    }

    public void Start()
    {
        if (!serialPort.IsOpen)
        {
            serialPort.Open();
        }
    }

    public void Stop()
    {
        if (serialPort.IsOpen)
        {
            serialPort.Close();
        }
    }

    private void OnDataReceived(
        object sender,
        SerialDataReceivedEventArgs e)
    {
        try
        {
            string line =
                serialPort
                    .ReadLine()
                    .Trim();

            if (line == "Success")
            {
                SuccessReceived?.Invoke();
            }
        }
        catch
        {
        }
    }

    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        disposed = true;

        Stop();

        serialPort.Dispose();
    }
}