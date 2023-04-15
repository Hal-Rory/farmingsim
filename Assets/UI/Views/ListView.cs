using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour
{
    protected Dictionary<string, GameObject> Cards = new Dictionary<string, GameObject>();
    [SerializeField] private ScrollRect Scroll;
    public int Count => Cards.Count;

    public virtual T AddCard<T>(string id, GameObject go, bool create = true)
    {
        if(Cards.TryGetValue(id, out GameObject cardObject))
        {
            return cardObject.GetComponent<T>();
        } else {
            GameObject new_obj;
            if (create)
            {
                new_obj = Instantiate(go, Scroll.content);
            } else
            {
                new_obj = go;
            }
            Cards.Add(id, new_obj);
            LayoutRebuilder.ForceRebuildLayoutImmediate(Scroll.content);
            return new_obj.GetComponent<T>();
        }
    }
    public virtual void RemoveCard(string id, bool destroy = true)
    {
        if (Cards.ContainsKey(id))
        {
            if (destroy)
            {
                Destroy(Cards[id].gameObject);
            }
            Cards.Remove(id);
            LayoutRebuilder.ForceRebuildLayoutImmediate(Scroll.content);
        }
    }
    public virtual void RemoveCards()
    {
        string[] keys = new string[Cards.Count];
        Cards.Keys.CopyTo(keys, 0);
        foreach (var item in keys)
        {
            RemoveCard(item);
        }
    }
    public void GoToBottom()
    {
        SetScrollPos(0);
    }
    public void GoToTop()
    {
        SetScrollPos(1);
    }
    private void SetScrollPos(float pos)
    {
        Scroll.verticalNormalizedPosition = pos;
    }
}
