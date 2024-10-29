using System.Collections.Generic;
using System.ComponentModel;

public class BackupJob : INotifyPropertyChanged
{
    private string _name;
    public string Name {
        get { return _name; }
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }
    public string SourceDirectory { get; set; }
    public string TargetDirectory { get; set; }
    public BackupType Type { get; set; }
    private int _id;
    public int Id
    {
        get { return _id; }
        set
        {
            if (_id != value)
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    
}

public enum BackupType
{
    Full,
    Differential
}