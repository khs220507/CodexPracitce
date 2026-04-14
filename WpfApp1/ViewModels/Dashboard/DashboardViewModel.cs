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
    private string _equipmentStatus = string.Empty;
    private string _lastInspectionTime = string.Empty;
    private string _activeAlarm = string.Empty;
    private string _lineName = string.Empty;

    public DashboardViewModel()
    {
        Title = "Dashboard";
        StatusMessage = "Production line overview and equipment summary";

        CurrentMode = SystemMode.Auto;
        TotalCount = 128;
        OkCount = 121;
        NgCount = 7;
        ConnectionSummary = "Vision / IO / Robot connected";
        LatestInspectionResult = "OK";
        EquipmentStatus = "Running";
        LastInspectionTime = "2026-04-14 21:48";
        ActiveAlarm = "No active alarms";
        LineName = "Front Assembly Line A";
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

    public string EquipmentStatus
    {
        get => _equipmentStatus;
        set => SetProperty(ref _equipmentStatus, value);
    }

    public string LastInspectionTime
    {
        get => _lastInspectionTime;
        set => SetProperty(ref _lastInspectionTime, value);
    }

    public string ActiveAlarm
    {
        get => _activeAlarm;
        set => SetProperty(ref _activeAlarm, value);
    }

    public string LineName
    {
        get => _lineName;
        set => SetProperty(ref _lineName, value);
    }

    public double YieldRate => TotalCount == 0 ? 0 : (double)OkCount / TotalCount * 100;
}
