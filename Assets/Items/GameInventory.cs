public class GameInventory : Inventory
{
    private static GameInventory _instance;
    public static GameInventory Instance { get => _instance?? (_instance = new GameInventory()); }
    public GameInventory()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}
