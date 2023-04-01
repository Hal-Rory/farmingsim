using UnityEngine;

public abstract class UIThemeData : ScriptableObject
{
    public string ID = string.Empty;
    public abstract void Copy(UIThemeData other);
    public override bool Equals(object other)
    {
        if(other is UIThemeData)
        {
            return ((UIThemeData)other).ID == ID;
        }
        if(other is string)
        {
            return ((string)other) == ID;
        }
        return base.Equals(other);
    }
}
