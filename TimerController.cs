using System;
using System.Windows.Threading;

namespace Redscreen;

public class TimerController
{
    private readonly DispatcherTimer timer;

    public int RemainingSeconds { get; private set; }

    public bool IsRunning { get; private set; }

    public event Action<int>? TimeChanged;

    public event Action? TimerFinished;

    public TimerController(int startSeconds)
    {
        RemainingSeconds = startSeconds;

        timer = new DispatcherTimer();

        timer.Interval = TimeSpan.FromSeconds(1);

        timer.Tick += OnTick;
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (!IsRunning)
        {
            return;
        }

        RemainingSeconds--;

        TimeChanged?.Invoke(RemainingSeconds);

        if (RemainingSeconds <= 0)
        {
            RemainingSeconds = 0;

            Stop();

            TimerFinished?.Invoke();
        }
    }

    public void Start()
    {
        if (IsRunning)
        {
            return;
        }

        IsRunning = true;

        timer.Start();
    }

    public void Stop()
    {
        IsRunning = false;

        timer.Stop();
    }

    public void Reset(int startSeconds)
    {
        Stop();

        RemainingSeconds = startSeconds;

        TimeChanged?.Invoke(RemainingSeconds);
    }

    public static string FormatTime(int seconds)
    {
        int minutes = seconds / 60;

        int remainSeconds = seconds % 60;

        return
            $"{minutes:00}:{remainSeconds:00}";
    }
}