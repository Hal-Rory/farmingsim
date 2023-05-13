using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class ToggleCollection : ToggleGroup
{
    public void SortToggles(Comparison<Toggle> comp)
    {
        m_Toggles.Sort(comp);
    }
    public IEnumerable<Toggle> GetToggles()
    {
        return m_Toggles;
    }

    public void SetToggleOn(Toggle toggle, bool sendCallback = true)
    {
        foreach (var item in m_Toggles)
        {
            if(item == toggle)
            {
                if (sendCallback)
                    item.isOn = true;
                else
                    item.SetIsOnWithoutNotify(true);
            }
        }
    }
    public void SetToggleOn(int index, bool sendCallback = true)
    {
        if(m_Toggles.Count > 0 && index.IsBetweenRange(0, m_Toggles.Count)) 
        { 
            if (sendCallback)
                m_Toggles[index].isOn = true;
            else
                m_Toggles[index].SetIsOnWithoutNotify(true);            
        }
    }
    public void NotifyToggleOn(int index, bool sendCallback = true)
    {
        // disable all toggles in the group
        for (var i = 0; i < m_Toggles.Count; i++)
        {
            if (i == index)
                continue;

            if (sendCallback)
                m_Toggles[i].isOn = false;
            else
                m_Toggles[i].SetIsOnWithoutNotify(false);
        }   
    }
}
