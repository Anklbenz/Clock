using System;
public interface IOnAlarmChangedTouch
{
     event Action<TimeSpan> AlarmChangedTouchEvent;
} 