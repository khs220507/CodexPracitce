using System.Windows;
using WpfApp1.ViewModels.Shell;

namespace WpfApp1;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var shellViewModel = new ShellViewModel();
        var window = new MainWindow
        {
            DataContext = shellViewModel
        };

        window.Show();
    }
}
