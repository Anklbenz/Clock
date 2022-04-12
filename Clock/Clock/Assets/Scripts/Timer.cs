using System;
using System.Threading.Tasks;

public class Timer : IOnTimeTick
{
    public event Action<TimeSpan> TickEvent;
    public DateTime CurrentTime => _currentTime;

    private readonly TimeSpan _tickTime = new TimeSpan(0, 0, 0, 0, 20);
    private readonly NetworkTimeGetter _networkTimeGetter;
    private readonly int _timeUpdateInterval;
    private DateTime _currentTime;

    public Timer(NetworkTimeGetter networkTimeGetter, int timeUpdateInterval){
        _timeUpdateInterval = timeUpdateInterval;
        _networkTimeGetter = networkTimeGetter;
    }

    public void TimerTick(){
        _currentTime = _currentTime.Add(_tickTime);
        TickEvent?.Invoke(_currentTime.TimeOfDay);
    }

    public async void TimerUpdate(){
        while (true){
            await Task.Run(SetServerTime);
            await Task.Delay(_timeUpdateInterval);
        }
    }

    private void SetServerTime(){
        var networkTime = _networkTimeGetter.Get();
        _currentTime = networkTime;
    }
}
