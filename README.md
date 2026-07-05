# Overview

A Windows application that displays a full-screen red warning screen in response to serial communication from an external device.

Developed for a school festival project.

## Features

- Listens for a serial signal from an external microcontroller.
- Displays a full-screen red warning screen when a predefined signal is received.
- Automatically returns to the standby timer screen after a configurable timeout.
- Configurable serial port settings via `appsettings.json`.

## Requirements

- Windows 10 or later
- .NET Runtime (tested with 10.0.301)
- Serial device connected via USB

## Configuration

Edit `appsettings.json`:

```json
{
  "SerialPort": {
    "PortName": "COM3",
    "BaudRate": 9600
  }
}
```
If you're having trouble communicating with the port, you may want to adjust this setting.
Place `appsettings.json` in the same directory as `Redscreen.exe`.

## Usage

1. Connect the external device.
2. Launch `Redscreen.exe`.
3. Send the string `Success` over the serial port.
1. After the timeout expires, the application automatically returns to the standby screen.


## Notes

1. This project was developed within three days for a school festival.

2. Although simple in concept, it required handling serial communication, timers, UI synchronization, configuration management, and deployment.

3. When the application receives the string

```text
Success
```

over the serial port, it displays the red warning screen.

