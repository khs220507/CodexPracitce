namespace WpfApp1.Common.Base;

public abstract class ViewModelBase : ObservableObject
{
    private string _title = string.Empty;
    private bool _isBusy;
    private string _statusMessage = string.Empty;

    public string Title
    {
        get => _title;
        protected set => SetProperty(ref _title, value);
    }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }
}
