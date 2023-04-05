using System;
using System.Collections;
using System.Collections.Generic;

namespace Entities
{
    public enum Stat { None, DrawSpeed, Armor, Agility, Speed }
    [Serializable]
    public class Stats
    {
        public Hashtable Pairs = new Hashtable();
        [Serializable]
        public struct Value
        {
            public float Amount;
            public Stat Stat;
            public Value(Stat stat, float amount)
            {
                Stat = stat;
                Amount = amount;
            }
            public override bool Equals(object obj)
            {
                return obj is Value ? Stat == ((Value)obj).Stat : base.Equals(obj);
            }
        }
        #region Constructors
        public Stats()
        {
            Pairs.Add(Stat.Agility, 0f);
            Pairs.Add(Stat.Speed, 0f);
            Pairs.Add(Stat.Armor, 0f);
            Pairs.Add(Stat.DrawSpeed, 0f);
        }
        public Stats(Stats other)
        {
            foreach (DictionaryEntry item in other.Pairs)
            {
                SetStat((Stat)item.Key, (float)item.Value);
            }
        }
        public Stats(params Value[] values)
        {
            foreach (var item in values)
            {
                SetStat(item);
            }
        }
        #endregion
        #region Modifiers
        public void Copy(Stats other)
        {
            Pairs.Clear();
            foreach (DictionaryEntry item in other.Pairs)
            {
                SetStat((Stat)item.Key, (float)item.Value);
            }
        }
        #endregion
        #region Setters
        /// <summary>
        /// Set value of stat or add and set value of stat
        /// </summary>
        /// <param name="item"></param>
        public void SetStat(Value item)
        {
            SetStat(item.Stat, item.Amount);
        }
        public void SetStat(params Value[] values)
        {
            foreach (var item in values)
            {
                SetStat(item);
            }
        }
        public void SetStat(Stat stat, float amount)
        {
            if (TryGetValue(stat, out float value))
            {
                Pairs[stat] = value + amount;
            }
            else
            {
                Pairs.Add(stat, amount);
            }
        }
        #endregion
        public override string ToString()
        {
            string stats = string.Empty;
            foreach (DictionaryEntry item in Pairs)
            {
                stats += $"{item.Key} : {item.Value}\n";
            }
            return stats;
        }
        public IEnumerable<string> LevelUpStats(params Value[] values)
        {            
            foreach (Value stat in values)
            {
                if (TryGetValue(stat.Stat, out float prev))
                {
                    SetStat(stat);
                    yield return $"{stat} : {prev} +{stat.Amount}";                    
                }
                else
                {
                    float added = stat.Amount;
                    SetStat(stat);
                    yield return $"+{stat} : {added}";                                        
                }
            }
        }
        public bool TryGetValue(Stat stat, out float value)
        {
            value = 0;
            if (Pairs.ContainsKey(stat))
            {
                value = (float)Pairs[stat];
                return true;
            }
            return false;
        }
        public bool Contains(Stats other)
        {
            foreach (DictionaryEntry item in other.Pairs)
            {
                if (!(other.TryGetValue((Stat)item.Key, out float amount) && amount == (float)item.Value))
                {
                    return false;
                }
            }
            return true;
        }
        public bool Contains(params Value[] other)
        {
            foreach (Value item in other)
            {
                if(!(TryGetValue(item.Stat, out float value) && value == item.Amount))
                {
                    return false;
                }
            }
            return true;
        }
        public override bool Equals(object obj)
        {
            if (obj is Stats other)
            {
                if(other.Pairs.Count != Pairs.Count) return false;                
                foreach (DictionaryEntry item in other.Pairs)
                {
                    if (!(TryGetValue((Stat)item.Key, out float amount) && amount == (float)item.Value))
                    {
                        return false;
                    }                    
                }
                return true;
            }
            else if(obj is float value)
            {
                foreach (DictionaryEntry item in Pairs)
                {
                    if ((float)item.Value != value)
                    {
                        return false;
                    }
                }
                return true;
            } else if(obj is Value[] stats)
            {
                foreach (Value item in stats)
                {
                    if (TryGetValue(item.Stat, out float amount) && amount != item.Amount)
                    {
                        return false;
                    }
                }
                return true;
            } else return base.Equals(obj);                        
        }
    }
}