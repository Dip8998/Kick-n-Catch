using System;
using KNC.Utilities;

namespace KNC.Core.Services
{
    public class EventService : GenericMonoSingleton<EventService>
    {
        public EventController OnKickStarted = new EventController();
        public EventController<float> OnPowerChanged = new EventController<float>();
        public EventController OnPowerReleased = new EventController();
        public EventController OnBallCaught = new EventController();
        public EventController OnBallMissed = new EventController();
        public EventController<int> OnScoreChanged = new EventController<int>();
        public EventController<int> OnHighScoreChanged = new EventController<int>();
        public EventController OnGameReset = new EventController();

        public void RaiseKickStarted() => OnKickStarted?.InvokeEvent();
        public void RaisePowerChanged(float v) => OnPowerChanged?.InvokeEvent(v);
        public void RaisePowerReleased() => OnPowerReleased?.InvokeEvent();
        public void RaiseBallCaught() => OnBallCaught?.InvokeEvent();
        public void RaiseBallMissed() => OnBallMissed?.InvokeEvent();
        public void RaiseScoreChanged(int s) => OnScoreChanged?.InvokeEvent(s);
        public void RaiseHighScoreChanged(int s) => OnHighScoreChanged?.InvokeEvent(s);
        public void RaiseGameReset() => OnGameReset?.InvokeEvent();
    }

    public class EventController
    {
        public event Action baseEvent;
        public void InvokeEvent() => baseEvent?.Invoke();
        public void AddListener(Action listener) => baseEvent += listener;
        public void RemoveListener(Action listener) => baseEvent -= listener;
    }

    public class EventController<T>
    {
        public event Action<T> baseEvent;
        public void InvokeEvent(T type) => baseEvent?.Invoke(type);
        public void AddListener(Action<T> listener) => baseEvent += listener;
        public void RemoveListener(Action<T> listener) => baseEvent -= listener;
    }
}
