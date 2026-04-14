using WpfApp1.Common.Base;
using WpfApp1.Models.Equipment;

namespace WpfApp1.ViewModels.Dashboard;

public sealed class DashboardViewModel : ViewModelBase
{
    private SystemMode _currentMode;
    private int _totalCount;
    private int _okCount;
    private int _ngCount;
    private string _connectionSummary = string.Empty;
    private string _latestInspectionResult = string.Empty;

    public DashboardViewModel()
    {
        Title = "Dashboard";
        StatusMessage = "Shell 구조 기준점";

        CurrentMode = SystemMode.Manual;
        TotalCount = 128;
        OkCount = 121;
        NgCount = 7;
        ConnectionSummary = "Vision / IO / Robot mock connected";
        LatestInspectionResult = "Latest result: OK";
    }

    public SystemMode CurrentMode
    {
        get => _currentMode;
        set => SetProperty(ref _currentMode, value);
    }

    public int TotalCount
    {
        get => _totalCount;
        set => SetProperty(ref _totalCount, value);
    }

    public int OkCount
    {
        get => _okCount;
        set => SetProperty(ref _okCount, value);
    }

    public int NgCount
    {
        get => _ngCount;
        set => SetProperty(ref _ngCount, value);
    }

    public string ConnectionSummary
    {
        get => _connectionSummary;
        set => SetProperty(ref _connectionSummary, value);
    }

    public string LatestInspectionResult
    {
        get => _latestInspectionResult;
        set => SetProperty(ref _latestInspectionResult, value);
    }
}
