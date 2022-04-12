using System;
using UnityEngine;

public interface IOnTouch
{
    event Action<Vector3> TouchPositionChangedEvent;
}