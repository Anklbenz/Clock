using System;

public interface IOnAlarmArrowRotate
{
    event Action<float> ArrowRotationChangedEvent;
}