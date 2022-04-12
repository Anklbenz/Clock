using System;
using UnityEngine;

public class AlarmSystem : IDisposable, IOnAlarmChangedTouch
{
    public event Action<TimeSpan> AlarmChangedTouchEvent;
    public event Action<bool> AlarmedEvent;

    private readonly IOnAlarmArrowRotate _onAlarmArrowRotate;
    private readonly IOnAlarmChangedInput _onAlarmChangedInput;
    private readonly IOnTimeTick _onTick;
    private readonly IOnAlarmButtonClick _onButtonClick;
    private TimeSpan _timeAlarm;
    private bool _alarmed;

    public AlarmSystem(IOnAlarmArrowRotate onAlarmArrowRotate, IOnAlarmChangedInput onAlarmChangedInput, IOnTimeTick onTick, IOnAlarmButtonClick onButtonClick){
        _onAlarmArrowRotate = onAlarmArrowRotate;
        _onAlarmChangedInput = onAlarmChangedInput;
        _onButtonClick = onButtonClick;
        _onTick = onTick;

        _onAlarmArrowRotate.ArrowRotationChangedEvent += AngleToTime;
        _onAlarmChangedInput.AlarmChangedInputEvent += TimeToAngle;
        _onButtonClick.AlarmButtonClickEvent += StartAlarm;
        _onTick.TickEvent += WakeupAlarm;
    }

    private void AngleToTime(float angle){
        var temp = angle / 30;
        var hour = (int) temp;

        var minutesTemp = temp - hour;
        var minutes = (int) Mathf.Round(minutesTemp * 60);

        _timeAlarm = new TimeSpan(0, hour, minutes, 0);
        AlarmChangedTouchEvent?.Invoke(_timeAlarm);
    }

    private void StartAlarm(){
        _alarmed = !_alarmed;
        AlarmedEvent?.Invoke(_alarmed);
    }

    private void TimeToAngle(TimeSpan hour){
        _timeAlarm = hour;
    }

    private void WakeupAlarm(TimeSpan time){
        if (!_alarmed) return;

        var hours = ConvertHourTo12Format(time.Hours);

        if (_timeAlarm.Hours == hours && _timeAlarm.Minutes == time.Minutes)
            Debug.Log($"Alarm Time RING-G-G-G!!!");
    }

    private int ConvertHourTo12Format(int hour){
        if (hour == 0)
            return 12;

        if (hour > 12)
            return hour -= 12;

        return hour;
    }

    public void Dispose(){
        _onAlarmArrowRotate.ArrowRotationChangedEvent -= AngleToTime;
        _onAlarmChangedInput.AlarmChangedInputEvent -= TimeToAngle;
        _onTick.TickEvent -= WakeupAlarm;
        _onButtonClick.AlarmButtonClickEvent -= StartAlarm;
    }
}
