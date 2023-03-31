using UnityEngine;
namespace Items
{
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Create New Weapon")]
    public class WeaponData : ObjData
    {
        [Range(1f, 10f)]
        public float Range = 10f;
        public float Attack = 1f;
        public int MaxCharges = 10;
        public float RechargeSeconds = 5;
        public float CooldownSeconds = 1;
    }
}