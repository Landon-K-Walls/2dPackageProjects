using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CCUtil.Controllers;

namespace CCUtil
{
    public class CountdownTimer
    {
        float _timeCap, _currentTime;
        bool _paused, _loop;

        public float time => _timeCap;
        public float timeLeft => _currentTime;
        public float percent => _currentTime / _timeCap;
        public float reversePercent => 1 - _currentTime / _timeCap;
        public bool paused => _paused;
        public bool looping => _loop;

        public UnityEvent onTimerExpired = new UnityEvent();

        public CountdownTimer(float time, bool startPaused = false, bool shouldLoop = false)
        {
            _timeCap = time;
            _currentTime = _timeCap;

            _paused = startPaused;
            _loop = shouldLoop;

            if (CountdownController.verify)
                CountdownController.AddTimer(this);

        }

        public void Update(float deltaTime)
        {
            if (!_paused)
                _currentTime -= deltaTime;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                if(onTimerExpired != null)
                    onTimerExpired.Invoke();

                if (!_loop)
                    Pause();
                else
                {
                    Reset();
                    Resume();
                }
            }
        }

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;
        public void Reset() => _currentTime = _timeCap;
        public void TimeStep(float time)
        {
            _currentTime += time;

            if (_currentTime > _timeCap) _currentTime = _timeCap;

            if (_currentTime <= 0)
            {
                _currentTime = 0;
                onTimerExpired.Invoke();
                Pause();

                if (_loop)
                {
                    Reset();
                    Resume();
                }
            }
        }
        public void SetLooping(bool loop) => _loop = loop;
        public void RegisterEvent(UnityAction call) => onTimerExpired.AddListener(call);
        public void ClearEvents() => onTimerExpired.RemoveAllListeners();
    }
}
