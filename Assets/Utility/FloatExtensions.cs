using UnityEngine;

public static class FloatExtensions
{
    /// <summary>
    /// Checks if between two values, includes equal to
    /// </summary>
    /// <param name="thisValue"></param>
    /// <param name="value1"></param>
    /// <param name="value2"></param>
    /// <returns></returns>
    public static bool IsBetweenRange(this float thisValue, float value1, float value2)
    {
        return thisValue >= Mathf.Min(value1, value2) && thisValue <= Mathf.Max(value1, value2);
    }
}
