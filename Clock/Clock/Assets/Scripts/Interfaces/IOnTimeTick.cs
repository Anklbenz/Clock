using System;

public interface IOnTimeTick
{
   event Action<TimeSpan> TickEvent;
}