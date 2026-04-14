using System.Windows.Input;
using WpfApp1.Common.Base;
using WpfApp1.Common.Commands;
using WpfApp1.ViewModels.Dashboard;

namespace WpfApp1.ViewModels.Shell;

public sealed class ShellViewModel : ViewModelBase
{
    private ViewModelBase _currentViewModel;
    private string _currentPageTitle;
    private string _globalConnectionSummary;

    public ShellViewModel()
    {
        DashboardViewModel = new DashboardViewModel();
        _currentViewModel = DashboardViewModel;
        _currentPageTitle = DashboardViewModel.Title;
        _globalConnectionSummary = DashboardViewModel.ConnectionSummary;

        Title = "Equipment Control Shell";
        StatusMessage = "Architecture baseline";

        NavigateDashboardCommand = new RelayCommand(NavigateDashboard);
    }

    public DashboardViewModel DashboardViewModel { get; }

    public ViewModelBase CurrentViewModel
    {
        get => _currentViewModel;
        private set => SetProperty(ref _currentViewModel, value);
    }

    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        private set => SetProperty(ref _currentPageTitle, value);
    }

    public string GlobalConnectionSummary
    {
        get => _globalConnectionSummary;
        private set => SetProperty(ref _globalConnectionSummary, value);
    }

    public ICommand NavigateDashboardCommand { get; }

    private void NavigateDashboard()
    {
        CurrentViewModel = DashboardViewModel;
        CurrentPageTitle = DashboardViewModel.Title;
        GlobalConnectionSummary = DashboardViewModel.ConnectionSummary;
    }
}
