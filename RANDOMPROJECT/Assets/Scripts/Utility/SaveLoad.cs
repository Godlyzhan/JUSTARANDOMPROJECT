using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;

public static class SaveLoad
{
    private const string FileName = "/savedGames.gd";
    public static SaveData SaveData = new SaveData();

    public static async Task SaveAsync()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        using (FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Create))
        {
            await Task.Run(() => binaryFormatter.Serialize(file, SaveData));
        }
    }

    public static async Task LoadAsync()
    {
        if (File.Exists(Application.persistentDataPath + FileName))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream file = File.Open(Application.persistentDataPath + FileName, FileMode.Open))
            {
                SaveData = await Task.Run(() => (SaveData)binaryFormatter.Deserialize(file));
        
                for (int i = 0; i < SaveData.CardPositions.Count; i++)
                {
                    SaveData.CardPositions[i] = ConvertToVector2(SaveData.CardPositions[i]);
                }
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

    // Conversion method from SerializableVector2 to Vector2
    public static Vector2 ConvertToVector2(SerializableVector2 serializableVector)
    {
        return new Vector2(serializableVector.x, serializableVector.y);
    }
}
