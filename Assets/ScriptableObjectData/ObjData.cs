using UnityEngine;
namespace Items
{
    public enum RANK_NAME { Bronze, Silver, Gold, Plantinum };
    public class ObjData : ScriptableObject, IFilterable
    {        
        public string Name;
        public string Description;
        public Sprite Display;
        [field: SerializeField] public string ID { get; protected set; }
        public int Cap = 1;
        public RANK_NAME Rank = 0;
        public bool Giftable;
    }
}