using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListView : MonoBehaviour
{
    protected Dictionary<string, GameObject> Cards = new Dictionary<string, GameObject>();
    [SerializeField] private RectTransform Container;
    public int Count => Cards.Count;

    public bool TryGetItem<T>(string id, out T item)
    {
        bool success = Cards.TryGetValue(id, out GameObject go);
        item = go.GetComponent<T>();
        return success;
    }

    public virtual T AddCard<T>(string id, GameObject go, bool create = true)
    {
        if(Cards.TryGetValue(id, out GameObject cardObject))
        {
            return cardObject.GetComponent<T>();
        } else {
            GameObject cardGO = GetOrCreateCard(go, create);
            Cards.Add(id, cardGO);
            LayoutRebuilder.ForceRebuildLayoutImmediate(Container);
            return cardGO.GetComponent<T>();
        }
    }
    protected virtual GameObject GetOrCreateCard(GameObject go, bool create)
    {
        if (create)
        {
            return Instantiate(go, Container);
        }
        else
        {
            return go;
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
            LayoutRebuilder.ForceRebuildLayoutImmediate(Container);
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
}
