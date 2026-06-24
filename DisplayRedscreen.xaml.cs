using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Redscreen;

public partial class DisplayRedscreen : Window
{
    private bool isShowing;

    public bool IsShowing
    {
        get
        {
            return isShowing;
        }
    }

    public DisplayRedscreen()
    {
        InitializeComponent();
    }

    public async Task ShowForSecondsAsync(
        int seconds,
        Window timerWindow)
    {
        if (isShowing)
        {
            return;
        }

        isShowing = true;

        Show();

        Activate();

        await Task.Delay(
            TimeSpan.FromSeconds(seconds));

        Hide();

        timerWindow.Topmost = true;

        timerWindow.Activate();

        timerWindow.Topmost = false;

        isShowing = false;
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