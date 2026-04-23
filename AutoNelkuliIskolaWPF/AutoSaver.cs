using AutoNelkuliIskolaWPF;
using System;
using System.ComponentModel;
using System.Windows.Threading;

public class AutoSaver : INotifyPropertyChanged
{
    private int _saveTime = 30; // másodperc

    private DispatcherTimer _saveTimer;
    private DispatcherTimer _displayTimer;
    private DateTime _nextSaveTime;

    private int _remainingSeconds;
    public int RemainingSeconds
    {
        get => _remainingSeconds;
        private set
        {
            _remainingSeconds = value;
            OnPropertyChanged(nameof(RemainingSeconds));
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public void StartAutoSave()
    {
        if (_saveTimer == null)
        {
            _saveTimer = new DispatcherTimer();
            _saveTimer.Tick += Save_Tick;
        }

        if (_displayTimer == null)
        {
            _displayTimer = new DispatcherTimer();
            _displayTimer.Tick += Display_Tick;
        }

        _saveTimer.Interval = TimeSpan.FromSeconds(_saveTime);
        _displayTimer.Interval = TimeSpan.FromMilliseconds(200);

        _nextSaveTime = DateTime.Now + _saveTimer.Interval;

        _saveTimer.Start();
        _displayTimer.Start();
    }

    public void StopAutoSave()
    {
        _saveTimer?.Stop();
        _displayTimer?.Stop();
    }

    private void Save_Tick(object sender, EventArgs e)
    {
        App.School.SaveToFile($"AutoSave_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");

        _nextSaveTime = DateTime.Now + _saveTimer.Interval;
    }

    private void Display_Tick(object sender, EventArgs e)
    {
        RemainingSeconds =
            (int)Math.Ceiling((_nextSaveTime - DateTime.Now).TotalSeconds);
    }
}