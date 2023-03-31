using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class SaveFile
{
    private static string SaveFolderPath = "Saves";
    private static string Extension => "rick";
    private static Encoding _encoder => Encoding.UTF8;

    private string FileName;
    private string CharacterChoice;
    
    private List<string> Checkpoints = new List<string>();
    private string LastCheckpoint = string.Empty;

    public SaveFile(string filename)
    {
        FileName = filename;
        
    }

    public void SetChar(string name)
    {
        CharacterChoice = name;
    }
    public void AddCheckpoint(string ID)
    {
        Checkpoints.Add(ID);
        LastCheckpoint = ID;
    }
    public IEnumerable<string> GetCheckpoints()
    {
        return Checkpoints;
    }
    public string GetLastCheckpoint() => LastCheckpoint;
    public override string ToString()
    {
        return $"{FileName}";
    }

    public void ToArray(BinaryWriter bw)
    {
        byte[] filename = _encoder.GetBytes(FileName);
        bw.Write(Convert.ToBase64String(filename));
        byte[] chara = _encoder.GetBytes(CharacterChoice);
        bw.Write(Convert.ToBase64String(chara));        
        string checkpoints = JsonConvert.SerializeObject(Checkpoints);
        bw.Write(checkpoints);
        bw.Write(LastCheckpoint);
    }

    #region Static Methods
    public static SaveFile FromArray(BinaryReader br)
    {
        byte[] fileName = Convert.FromBase64String(br.ReadString());
        SaveFile save = new SaveFile(_encoder.GetString(fileName));
        byte[] charaChoice = Convert.FromBase64String(br.ReadString());
        save.SetChar(_encoder.GetString(charaChoice));
        save.Checkpoints = JsonConvert.DeserializeObject<List<string>>(br.ReadString());
        save.LastCheckpoint = br.ReadString();
        return save;
    }
    private static string FilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, SaveFolderPath, fileName);
    }
    public static void Save(SaveFile save)
    {
        var fileFullPath = FilePath($"{save.FileName}.{Extension}");
        string directoryPath = FilePath(string.Empty);
        if (!Directory.Exists(directoryPath))
        {
            Debug.Log($"Save directory not found, creating {directoryPath}");
            Directory.CreateDirectory(directoryPath);
        }
        using (BinaryWriter bw = new BinaryWriter(File.Open(fileFullPath, FileMode.OpenOrCreate)))
        {
            save.ToArray(bw);
        }
        Debug.Log($"Saved Binary to {fileFullPath}");
    }

    public static bool Load(string fileName, out SaveFile save)
    {
        var fileFullPath = FilePath($"{fileName}.{Extension}");
        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileFullPath))
        {
            using (BinaryReader br = new BinaryReader(File.Open(fileFullPath, FileMode.Open)))
            {
                save = FromArray(br);
            }
            return true;
        } else
        {
            Debug.Log($"No such save found: {fileName}, creating new");
            save = new SaveFile(fileName);
            return false;
        }
    }
    #endregion
}
