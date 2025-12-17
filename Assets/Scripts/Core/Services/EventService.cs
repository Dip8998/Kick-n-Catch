using System;
using KNC.Utilities;

namespace KNC.Core.Services
{
    public class EventService : GenericMonoSingleton<EventService>
    {
        public readonly EventController OnKickStarted = new();
        public readonly EventController<float> OnPowerChanged = new();
        public readonly EventController OnPowerReleased = new();
        public readonly EventController OnBallCaught = new();
        public readonly EventController OnBallMissed = new();
        public readonly EventController<int> OnScoreChanged = new();
        public readonly EventController<int> OnHighScoreChanged = new();
        public readonly EventController OnGameReset = new();
        public readonly EventController OnGameOver = new();

        public void RaiseKickStarted() => OnKickStarted.InvokeEvent();
        public void RaisePowerChanged(float v) => OnPowerChanged.InvokeEvent(v);
        public void RaisePowerReleased() => OnPowerReleased.InvokeEvent();
        public void RaiseBallCaught() => OnBallCaught.InvokeEvent();
        public void RaiseBallMissed() => OnBallMissed.InvokeEvent();
        public void RaiseScoreChanged(int s) => OnScoreChanged.InvokeEvent(s);
        public void RaiseHighScoreChanged(int s) => OnHighScoreChanged.InvokeEvent(s);
        public void RaiseGameReset() => OnGameReset.InvokeEvent();
        public void RaiseGameOver() => OnGameOver.InvokeEvent();
    }

    public class EventController
    {
        private event Action handlers;
        public void InvokeEvent() => handlers?.Invoke();
        public void AddListener(Action listener) => handlers += listener;
        public void RemoveListener(Action listener) => handlers -= listener;
    }

    public class EventController<T>
    {
        private event Action<T> handlers;
        public void InvokeEvent(T value) => handlers?.Invoke(value);
        public void AddListener(Action<T> listener) => handlers += listener;
        public void RemoveListener(Action<T> listener) => handlers -= listener;
    }
}
