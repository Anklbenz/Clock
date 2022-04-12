using System;
using UnityEngine.UI;

public class UIDrawer : IDisposable
{
    private readonly Text _timeText, _alarmLabel;
    private readonly InputField  _alarmHourText, _alarmMinuteText;
    private readonly IOnTimeTick _onTimeTick;
    
    private readonly AlarmSystem _alarmSystem;
    public UIDrawer(Text timeText, InputField alarmHourText, InputField alarmMinuteText, Text alarmLabel, AlarmSystem alarmSystem, IOnTimeTick onTimeTick){
        _timeText = timeText;
        _alarmLabel = alarmLabel;
        _alarmHourText = alarmHourText;
        _alarmMinuteText = alarmMinuteText;
        
        _alarmSystem = alarmSystem;
        _onTimeTick =onTimeTick;

        _alarmSystem.AlarmChangedTouchEvent += UpdateAlarmInputBox;
        _alarmSystem.AlarmedEvent += AlarmLabelSetEnable;
        _onTimeTick.TickEvent += RefreshTime;
    }

    private void RefreshTime(TimeSpan time){
        _timeText.text = time.ToString(@"h\:mm\:ss");
    }

    private void UpdateAlarmInputBox(TimeSpan alarm){
        _alarmHourText.text = alarm.Hours.ToString();
        _alarmMinuteText.text = alarm.Minutes.ToString();
    }

    private void AlarmLabelSetEnable(bool enabled){
        _alarmLabel.gameObject.SetActive( enabled);
    }
    
    public void Dispose(){
        _alarmSystem.AlarmChangedTouchEvent -= UpdateAlarmInputBox;
        _onTimeTick.TickEvent -= RefreshTime;
    }
}
