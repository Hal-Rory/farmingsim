using UnityEngine;
namespace Items
{
    public enum RANK_NAME { Bronze, Silver, Gold, Platinum };
    public class ObjData : ScriptableObject, IFilterable
    {        
        public string Name;
        public string Description;
        public Sprite Display;
        [field: SerializeField] public string ID { get; protected set; }
        public RANK_NAME Rank = 0;
        public bool Sellable;
        public void SetID(string ID)
        {
            this.ID = ID;
        }
    }
}