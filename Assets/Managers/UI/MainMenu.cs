using UnityEngine;

public class MainMenu : UIPage
{
    private bool IsOpen;
    [SerializeField] private ToggleCollection Tabs;
    [SerializeField] private GameObject[] Windows;
    protected override void Start()
    {
        base.Start();
        GameManager.Instance.InputManager.RegisterMenuListener(OpenMenu);        
    }
    private void OnEnable()
    {
        Tabs.SortToggles((a, b) =>
        {
            if (a == null && b == null) return 0;
            if (a == null && b != null) return 1;
            if (a != null && b == null) return -1;
            return a.transform.GetSiblingIndex() > b.transform.GetSiblingIndex() ? 1 : -1;
        });
        Tabs.SetToggleOn(0);
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        GameManager.Instance.InputManager.UnregisterMenuListener(OpenMenu);
    }
    private void OpenMenu(bool interact)
    {
        if (interact)
        {
            if (IsOpen)
            {
                IsOpen = false;
                CloseFocus();
            }
            else
            {
                IsOpen= true;
                OpenFocus();
            }
        }
    }
    public void OpenWindow(GameObject window)
    {
        foreach (var item in Windows)
        {
            if (item != window)
            {
                item.SetActive(false);
            }
            else
            {
                item.SetActive(true);
            }
        }
    }
}
