using System;
using System.Windows;
using System.Windows.Input;

namespace Redscreen;

public partial class MainWindow : Window
{
    private readonly AppConfig config;

    private readonly TimerController timerController;

    private readonly DisplayRedscreen redScreen;

    private readonly DetectSignalFromUSBPort serialMonitor;

    public MainWindow()
    {
        InitializeComponent();

        config =
            AppConfig.Load("appsettings.json");

        timerController =
            new TimerController(
                config.TimerStartSeconds);

        redScreen =
            new DisplayRedscreen();

        serialMonitor =
            new DetectSignalFromUSBPort(
                config.PortName,
                config.BaudRate);

        TimerText.Text =
            TimerController.FormatTime(
                config.TimerStartSeconds);

        timerController.TimeChanged +=
            OnTimeChanged;

        timerController.TimerFinished +=
            OnTimerFinished;

        serialMonitor.SuccessReceived +=
            OnSuccessReceived;

        Loaded += MainWindow_Loaded;

        Closed += MainWindow_Closed;
    }

    private void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e)
    {
        try
        {
            serialMonitor.Start();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Serial Error");
        }
    }

    private void MainWindow_Closed(
        object sender,
        EventArgs e)
    {
        serialMonitor.Dispose();
    }

    private void OnTimeChanged(
        int remainingSeconds)
    {
        Dispatcher.Invoke(() =>
        {
            TimerText.Text =
                TimerController.FormatTime(
                    remainingSeconds);
        });
    }

    private void OnTimerFinished()
    {
        Dispatcher.Invoke(() =>
        {
            MessageText.Visibility =
                Visibility.Collapsed;

            ResetButton.Visibility =
                Visibility.Visible;
        });
    }

    private async void OnSuccessReceived()
    {
        await Dispatcher.InvokeAsync(async () =>
        {
            if (redScreen.IsShowing)
            {
                return;
            }

            await redScreen.ShowForSecondsAsync(
                config.RedScreenDurationSeconds,
                this);
        });
    }

    private void Window_MouseLeftButtonDown(
        object sender,
        MouseButtonEventArgs e)
    {
        if (timerController.IsRunning)
        {
            return;
        }

        if (ResetButton.Visibility ==
            Visibility.Visible)
        {
            return;
        }

        MessageText.Visibility =
            Visibility.Collapsed;

        timerController.Start();
    }

    private void ResetButton_Click(
        object sender,
        RoutedEventArgs e)
    {
        timerController.Reset(
            config.TimerStartSeconds);

        TimerText.Text =
            TimerController.FormatTime(
                config.TimerStartSeconds);

        ResetButton.Visibility =
            Visibility.Collapsed;

        MessageText.Visibility =
            Visibility.Visible;
    }

    private void Window_KeyDown(
        object sender,
        KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Application.Current.Shutdown();
        }
    }
}