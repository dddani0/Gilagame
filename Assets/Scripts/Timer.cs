namespace DefaultNamespace
{
    public class Timer
    {
        private float _defaultTimer;
        private float _timerValue;

        public Timer(float defaultTimer)
        {
            _defaultTimer = defaultTimer;
            _timerValue = _defaultTimer;
        }

        public void DecreaseTimer(float value)
        {
            _timerValue -= value;
        }

        public void SetNewTimerValue(float newValue)
        {
            _defaultTimer = newValue;
            ResetTimer();
        }

        public void ResetTimer() => _timerValue = _defaultTimer;

        public bool IsCooldown() => _timerValue > 0;
        public float GetTimer() => _timerValue;
    }
}