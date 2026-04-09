using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CCUtil.Controllers
{
    public class CountdownController : Singleton<CountdownController>
    {
        List<CountdownTimer> _timers;

        protected override void Awake()
        {
            base.Awake();

            _timers = new List<CountdownTimer>();
        }

        private void Update()
        {
            for(int i = 0; i < _timers.Count; i++)
            {
                if (_timers[i] != null)
                    _timers[i].Update(Time.deltaTime);
            }
        }

        private void RemoveNulls()
        {
            foreach (CountdownTimer timer in _timers)
            {
                if (timer == null)
                    RemoveTimer(timer);
            }
        }

        public static void AddTimer(CountdownTimer timer) => instance._timers.Add(timer);
        public static void RemoveTimer(CountdownTimer timer) => instance._timers.Remove(timer);
        public static void Contains(CountdownTimer timer) => instance._timers.Contains(timer);
        public static void ClearAll() => instance._timers.Clear();
    }
}
