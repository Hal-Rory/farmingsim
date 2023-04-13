using UnityEngine;

public static class IntExtensions
{    
    /// <summary>
    /// Loops the value t, so that it is never larger than length and never smaller than
    /// </summary>
    /// <param name="i"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static int Repeat(this int i, int length)
    {
        return Mathf.FloorToInt(Mathf.Repeat(i, length));
    }
}
