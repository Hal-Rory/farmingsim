using GameTime;
using System.Collections;

public interface ITimeManager
{
    public TimeStruct CurrentTime { get; }
    public bool RegisterListener(ITimeListener listener);
    public bool UnregisterListener(ITimeListener listener);

    public IEnumerator Tick(bool single = false);
    public string DisplayTime();
    public void SetTick(bool canTick);
}
