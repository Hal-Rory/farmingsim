using UnityEngine;
using static UIManager;

public abstract class UIPage : MonoBehaviour, IFilterable
{
    [field: SerializeField] public UI_STATE State {get; private set;}
    [field:SerializeField] public string ID {get; private set;}

    protected virtual void Start()
    {
        if (!GameManager.Instance.UIManager.RegisterPage(this))
        {
            Debug.LogError($"Could not register {nameof(UIPage)}: {name}");
        }
    }

    protected virtual void OnDestroy()
    {
        if (!GameManager.Instance.UIManager.UnregisterPage(this))
        {
            Debug.LogError($"Could not unregister {nameof(UIPage)}: {name}");
        }
    }
    public void OpenFocus()
    {
        GameManager.Instance.UIManager.SetFocused(ID);
    }
    public void CloseFocus()
    {
        GameManager.Instance.UIManager.CloseFocused(ID);
    }
}
