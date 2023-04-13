using UnityEngine;

public class Wallet
{
    [SerializeField] private int Max = 9999;
    public int Balance { get; private set; }
    public Wallet(int balance = 0)
    {
        Balance = balance;
    }
    public bool Modify(int amount)
    {
        if (amount < 0 && !CheckAmount(Mathf.Abs(amount))) return false;
        Balance = Mathf.Clamp(Balance + amount, 0, Max);
        return true;
    }
    public bool CheckAmount(int amount)
    {
        return Balance >= amount;
    }
    public override string ToString()
    {
        return $"{Balance}";
    }
}
