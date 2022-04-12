using System;
using UnityEngine;

public class ArrowSystem : IDisposable
{
    private const float HOURS_IN_DEGREES = 30f;
    private const float MINUTES_IN_DEGREES = 6f;
    private const float SECONDS_IN_DEGREES = 6f;
    private readonly Transform _hour, _minute, _second, _alarm;
    private readonly IOnTouch _onTouch;
    private readonly IOnAlarmChangedInput _onInput;

    public ArrowSystem(Transform hour, Transform minute, Transform seconds, Transform alarm, IOnTouch onTouch, IOnAlarmChangedInput onInput){
        _hour = hour;
        _minute = minute;
        _second = seconds;
        _alarm = alarm;
        _onTouch = onTouch;
        _onInput = onInput;

        _onTouch.TouchPositionChangedEvent += OnTouchRotate;
        _onInput.AlarmChangedInputEvent += OnInputRotate;
    }

    public void Rotate(TimeSpan now){
        _hour.localRotation = Quaternion.Euler(0f, (float) now.TotalHours * HOURS_IN_DEGREES, 0f);
        _minute.localRotation = Quaternion.Euler(0f, (float) now.TotalMinutes * MINUTES_IN_DEGREES, 0f);
        _second.localRotation = Quaternion.Euler(0f, (float) now.TotalSeconds * SECONDS_IN_DEGREES, 0f);
    }

    private void OnTouchRotate(Vector3 touchPoint){
        _alarm.LookAt(touchPoint);
        _alarm.eulerAngles = new Vector3(0, _alarm.eulerAngles.y, 0);
    }

    private void OnInputRotate(TimeSpan time){
        _alarm.localRotation = Quaternion.Euler(0f, (float) time.TotalHours * HOURS_IN_DEGREES, 0f);
    }

    public void Dispose(){
        _onTouch.TouchPositionChangedEvent -= OnTouchRotate;
        _onInput.AlarmChangedInputEvent -= OnInputRotate;
    }
}