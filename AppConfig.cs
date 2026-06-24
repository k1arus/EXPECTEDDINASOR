using System.IO;
using System.Text.Json;

namespace Redscreen;

public class AppConfig
{
    public string PortName { get; set; } = "COM3";

    public int BaudRate { get; set; } = 115200;

    public int RedScreenDurationSeconds { get; set; } = 10;

    public int TimerStartSeconds { get; set; } = 300;

    public static AppConfig Load(string path)
    {
        if (!File.Exists(path))
        {
            return new AppConfig();
        }

        string json = File.ReadAllText(path);

        AppConfig? config =
            JsonSerializer.Deserialize<AppConfig>(json);

        return config ?? new AppConfig();
    }
}