using UnityEngine;

public class HUDUI : UIPage
{
    [SerializeField] private TimeUI Time;
    [SerializeField] private Card Item;
    [SerializeField] private Card Tool;
    [SerializeField] private Card Money;

    [SerializeField] private ButtonCard Pause;
    [SerializeField] private ButtonCard Play;
    [SerializeField] private ButtonCard Fast;

    protected override void Start()
    {
        base.Start();
        Time.Register();
        GameManager.Instance.OnMoneyUpdated += DoMoneyUpdated;
        GameManager.Instance.FarmToolManager.RegisterListener(SetCurrentToolActive);
        GameManager.Instance.OnCurrentItemUpdated += DoCurrentItemUpdated;

        SetCurrentToolActive(GameManager.Instance.FarmToolManager.GetCurrentTool());
        DoCurrentItemUpdated(GameManager.Instance.GetItem());
        DoMoneyUpdated(GameManager.Instance.WalletBalance);

        Pause.SetAction(TryPauseTime);
        Play.SetAction(TryStartTime);
        Fast.SetAction(TrySpeedupTime);

        DoTimeChanged(ITimeManager.Instance.State);
        GameManager.Instance.OnTimeUpdated += DoTimeChanged;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        Time.Unregister();
        GameManager.Instance.OnMoneyUpdated -= DoMoneyUpdated;
        GameManager.Instance.FarmToolManager.UnregisterListener(SetCurrentToolActive);
        GameManager.Instance.OnCurrentItemUpdated -= DoCurrentItemUpdated;

        Pause.SetAction(null);
        Play.SetAction(null);
        Fast.SetAction(null);
    }
    private void DoCurrentItemUpdated(Item obj)
    {
        Item.SetLabel($"{(obj != null ? "x" + obj?.Amount : string.Empty)}");
        Item.SetIcon(obj?.Data.Display);
    }
    private void DoMoneyUpdated(int amount)
    {
        //can use amount to increment label via coroutine
        Money.SetLabel($"${GameManager.Instance.WalletBalance}");
    }
    private void SetCurrentToolActive(IFarmToolCollection tool)
    {
        Tool.SetLabel(tool != null ? $"{tool.Data.Name}" : "None");
        Tool.SetIcon(tool?.Data.Display);
    }

    private void DoTimeChanged(ITimeManager.TIME_STATE state)
    {
        Play.gameObject.SetActive(state == ITimeManager.TIME_STATE.playing);
        Pause.gameObject.SetActive(state == ITimeManager.TIME_STATE.paused);
        Fast.gameObject.SetActive(state == ITimeManager.TIME_STATE.fast);
    }

    private void TryPauseTime()
    {
        GameManager.Instance.PauseTime();
    }
    private void TryStartTime()
    {
        GameManager.Instance.PlayRegularTime();
    }
    private void TrySpeedupTime()
    {
        GameManager.Instance.SpeedUpTime();
    }
}
