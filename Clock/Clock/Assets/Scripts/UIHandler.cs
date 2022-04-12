using System;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour, IOnAlarmChangedInput, IOnAlarmButtonClick
{
    public event Action<TimeSpan> AlarmChangedInputEvent;
    public event Action AlarmButtonClickEvent;

    [SerializeField] private InputField hoursField, minutesField;

    public void InputFieldsChanged(){
        if (!Int32.TryParse(hoursField.text, out var hour))
            hour = 0;

        if (!Int32.TryParse(minutesField.text, out var minute))
            minute = 0;

        if (hour > 12){
            hour = 12;
            hoursField.text = "12";
        }

        if (minute > 59){
            minute = 59;
            minutesField.text = "59";
        }
        var time = new TimeSpan(hour, minute, 0);
        AlarmChangedInputEvent?.Invoke(time);
    }
    
    public void OnAlarmButtonClick(){
       AlarmButtonClickEvent?.Invoke();
    }
    
}
