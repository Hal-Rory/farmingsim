using UnityEngine;
namespace Items
{
    public enum TOOL_TYPE { Water, Dig, Cut, Hands, None };
    [CreateAssetMenu(fileName = "New Tool", menuName = "Create New Tool")]
    public class ToolData : WeaponData
    {
        public override SELECTABLE_TYPE DataType { get; } = SELECTABLE_TYPE.tool;
        public TOOL_TYPE ToolType = TOOL_TYPE.Hands;
    }
}
