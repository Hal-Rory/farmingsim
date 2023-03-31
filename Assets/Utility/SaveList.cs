using System.Collections.Generic;

public class SaveList
{
    public static List<string> SaveFiles = new List<string>() { "One", "Two", "Three" };
    public delegate void SaveListUpdated();
    public static event SaveListUpdated OnNewSave;

    public static void LoadSaves()
    {

    }
}
