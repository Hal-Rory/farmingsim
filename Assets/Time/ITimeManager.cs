using GameTime;
using System.Collections;

public interface ITimeManager
{
    public static ITimeManager Instance;
    public enum TIME_STATE { playing, paused, fast };
    public TIME_STATE State { get; }
    public TimeStruct CurrentTime { get; }
    public bool CanTick { get; }
    public bool RegisterListener(ITimeListener listener);
    public bool UnregisterListener(ITimeListener listener);

    public IEnumerator Tick(int tick = 1, bool single = false);
    public string DisplayTime();
    public void SetTick(bool canTick);
    public void SetTimeDelta(float amount, TIME_STATE state);
    public float TimeDelta { get; }
}
