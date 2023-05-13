using System;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public enum UI_STATE { Focused, Base, Paused };
    private Dictionary<string, UIPage> PageStates = new Dictionary<string, UIPage>();
    public event Action<UI_STATE> OnFocusedChanged;
    private string CurrentFocus;
    private UI_STATE State;
    private void CloseBasePages()
    {
        foreach (var page in PageStates)
        {
            if (page.Value.State == UI_STATE.Base)
            {
                page.Value.gameObject.SetActive(false);
            }
        }
    }
    private void OpenBasePages()
    {                
        foreach (var page in PageStates) 
        {            
            if (page.Value.State == UI_STATE.Base)
            {
                page.Value.gameObject.SetActive(true);
            } else
            {
                page.Value.gameObject.SetActive(false);
            }
        }
    }
    public void SetBaseFocus()
    {
        CurrentFocus = string.Empty;
        OpenBasePages();
        State = UI_STATE.Base;
        OnFocusedChanged?.Invoke(State);
    }
    public void SetFocused(string current)
    {
        if (!PageStates.TryGetValue(current, out UIPage page)) return;
        if (page.State == UI_STATE.Paused)
        {
            CloseBasePages();
            if(!string.IsNullOrEmpty(CurrentFocus)) PageStates[CurrentFocus].gameObject.SetActive(false);
            State = UI_STATE.Paused;
            OnFocusedChanged?.Invoke(State);
            page.gameObject.SetActive(true);
            return;
        }
        else if (string.IsNullOrEmpty(CurrentFocus)) {
            CloseBasePages();
            CurrentFocus = current;
            State = UI_STATE.Focused;
            OnFocusedChanged?.Invoke(State);
            page.gameObject.SetActive(true);
        }
    }
    public void CloseFocused(string current)
    {
        if (!PageStates.TryGetValue(current, out UIPage page)) return;
        if (page.State == UI_STATE.Paused)
        {                        
            page.gameObject.SetActive(false);
            if (!string.IsNullOrEmpty(CurrentFocus))
            {
                PageStates[CurrentFocus].gameObject.SetActive(true);
                State = UI_STATE.Focused;
                OnFocusedChanged?.Invoke(State);
            }
            return;
        }
        else if (CurrentFocus == current)
        {
            page.gameObject.SetActive(false);
            SetBaseFocus();
        }
    }

    public bool RegisterPage(UIPage page)
    {
        page.gameObject.SetActive(page.State == State);
        return PageStates.TryAdd(page.ID, page);
    }
    public bool UnregisterPage(UIPage page)
    {
        return PageStates.Remove(page.ID);
    }
}
