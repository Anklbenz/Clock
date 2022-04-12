using System;
using UnityEngine;

public class TouchController : IOnTouch, IOnAlarmArrowRotate
{
    public event Action<Vector3> TouchPositionChangedEvent;
    public event Action<float> ArrowRotationChangedEvent;

    private readonly BoxCollider _touchCollider;
    private readonly Camera _camera;
    private bool _isTouched;
    private RaycastHit _hit;

    public TouchController(BoxCollider touchCollider, Camera camera){
        _touchCollider = touchCollider;
        _camera = camera;
    }

    public void TouchCheck(){
        if (Input.GetKey(KeyCode.Mouse0)){
            var ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit) && _hit.collider == _touchCollider)
                _isTouched = true;
        }
        else{
            _isTouched = false;
        }

        if (!_isTouched) return;

        SendTouchPosition();
        SendArrowRotation();
    }

    private void SendTouchPosition(){
        var touchPoint = _camera.ScreenPointToRay(Input.mousePosition).GetPoint(0);
        TouchPositionChangedEvent?.Invoke(touchPoint);
    }

    private void SendArrowRotation(){
        var angle = _touchCollider.transform.eulerAngles.y;
        if (angle < 0)
            angle = 360 + angle;
        ArrowRotationChangedEvent?.Invoke(angle);
    }
}
