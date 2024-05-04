using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public static class SaveLoad
{
    private const string FileName = "/savedGames.gd";
    private static List<SaveData> saveData = new List<SaveData>();

    public static async Task SaveAsync()
    {
        saveData.Add(SaveData.current);
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Create))
        {
            await Task.Run(() => binaryFormatter.Serialize(file, SaveLoad.saveData));
        }
    }

    public static async Task LoadAsync()
    {
        if (File.Exists(Application.persistentDataPath + FileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Open))
            {
                saveData = await Task.Run(() => (List<SaveData>)binaryFormatter.Deserialize(file));
            }
        }
    }

    public static async Task DeleteSaveDataAsync()
    {
        string filePath = Application.persistentDataPath + FileName;
        if (File.Exists(filePath))
        {
            await Task.Run(() => File.Delete(filePath));
        }
    }
}
