using UnityEngine;
using UnityEngine.UI;

public class ClockMain : MonoBehaviour
{
    
    [SerializeField] private Transform hourArrow, minuteArrow, secondsArrow;
    [Space]
    [SerializeField] private BoxCollider alarmArrow;
    [Space]
    [SerializeField] private string[] ntpAddress;
    [SerializeField] private int timeUpdateInterval;
    [Space]
    [SerializeField] private Text timeTextLabel, alarmLabel;
    [SerializeField] private InputField alarmHourField, alarmMinuteField;
    [SerializeField] private UIHandler uiHandler;
    
    private Timer _timer;
    private TouchController _touchController;
    private AlarmSystem _alarmSystem;
    private NetworkTimeGetter _networkTimeGetter;
    private ArrowSystem _arrowSystem;
    private UIDrawer _uiDrawer;

    private void Awake(){
       
        _touchController = new TouchController(alarmArrow, Camera.main);
        _arrowSystem = new ArrowSystem(hourArrow, minuteArrow, secondsArrow, alarmArrow.transform, _touchController, uiHandler);
        _networkTimeGetter = new NetworkTimeGetter(ntpAddress);
        _timer = new Timer(_networkTimeGetter, timeUpdateInterval);
        _alarmSystem = new AlarmSystem(_touchController, uiHandler, _timer,uiHandler);
        _uiDrawer = new UIDrawer(timeTextLabel, alarmHourField, alarmMinuteField, alarmLabel, _alarmSystem, _timer);
        
        _timer.TimerUpdate();
    }
    private void Update(){
        _touchController.TouchCheck();
    }

    private void FixedUpdate(){
        _timer.TimerTick();
        _arrowSystem.Rotate(_timer.CurrentTime.TimeOfDay);
    }
}