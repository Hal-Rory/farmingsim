using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GameTime
{    
    public class TimeManager : ITimeManager
    {
        public TimeStruct _currentTime;
        private bool CanTick;
        private HashSet<ITimeListener> TimeListeners = new HashSet<ITimeListener>();
        private float TimeDelta = 1;

        public TimeStruct CurrentTime
        {
            get => _currentTime; 
        }
        public TimeManager(float delta,int time, int date, int month, int year)
        {
            TimeDelta = delta;
            _currentTime = new TimeStruct(time, date, month, year);
        }
        public TimeManager(float delta, TimeStruct other)
        {
            TimeDelta = delta;
            _currentTime = other;
        }

        public void SetTick(bool canTick)
        {
            CanTick = canTick;
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