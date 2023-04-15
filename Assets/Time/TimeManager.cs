using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameTime
{

    public class TimeManager : ITimeManager
    {
        public TimeStruct _currentTime;
        public bool CanTick { get; private set; }
        private HashSet<ITimeListener> TimeListeners = new HashSet<ITimeListener>();
        public float TimeDelta { get; private set; } = 1;

        public TimeStruct CurrentTime
        {
            get => _currentTime; 
        }

        public ITimeManager.TIME_STATE State { get; private set; }
        public TimeManager()
        {
            ITimeManager.Instance = this;
        }
        public TimeManager(float delta, TimeStruct other): this()
        {
            TimeDelta = delta;
            _currentTime = other;
        }

        public void SetTick(bool canTick)
        {
            CanTick = canTick;
            if(!CanTick) State = ITimeManager.TIME_STATE.paused;
        }
        public void SetTimeDelta(float amount, ITimeManager.TIME_STATE state)
        {
            State = state;
            TimeDelta = Mathf.Clamp(amount, .01f, amount);
        }
        public bool RegisterListener(ITimeListener listener)
        {
            return TimeListeners.Add(listener);
        }
        public bool UnregisterListener(ITimeListener listener)
        {
            return TimeListeners.Remove(listener);
        }

        public IEnumerator Tick(bool single = false)
        {
            while(CanTick)
            {
                yield return new WaitForSeconds(TimeDelta);
                if (CanTick)
                {
                    _currentTime.AddTime(1);

                    foreach (var item in TimeListeners)
                    {
                        item.ClockUpdate(_currentTime);
                    }
                    if (single)
                    {
                        CanTick = false;
                    }
                }
            }
        }

        public string DisplayTime()
        {
            return _currentTime.ToString();
        }
    }
}