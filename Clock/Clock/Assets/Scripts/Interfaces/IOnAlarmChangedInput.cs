using System;

public interface IOnAlarmChangedInput
{
    event Action<TimeSpan> AlarmChangedInputEvent;
}