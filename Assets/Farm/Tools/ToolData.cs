using UnityEngine;
namespace Items
{
    public enum TOOL_TYPE { Water, Dig, Cut, Hands };
    [CreateAssetMenu(fileName = "New Tool", menuName = "Create New Tool")]
    public class ToolData : WeaponData
    {
        public TOOL_TYPE ToolType = TOOL_TYPE.Hands;
    }
}
