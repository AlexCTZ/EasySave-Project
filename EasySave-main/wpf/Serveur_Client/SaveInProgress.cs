using System.ComponentModel;

public class SaveInProgress : INotifyPropertyChanged
{
    private string backupAction;
    public string BackupAction
    {
        get { return backupAction; }
        set
        {
            if (value != backupAction)
            {
                backupAction = value;
                OnPropertyChanged(nameof(BackupAction));
            }
        }
    }

    private string selectedSave;
    public string SelectedSave
    {
        get { return selectedSave; }
        set
        {
            if (value != selectedSave)
            {
                selectedSave = value;
                OnPropertyChanged(nameof(SelectedSave));
            }
        }
    }

    private int progress;
    public int Progress
    {
        get { return progress; }
        set
        {
            if (value != progress)
            {
                progress = value;
                OnPropertyChanged(nameof(Progress));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
