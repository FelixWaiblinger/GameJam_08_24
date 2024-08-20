using System.IO;
using UnityEngine;

public class ReaderWriterJSON : MonoBehaviour
{
    private static string persistentPath = Application.persistentDataPath;

    public static void SaveToJSON(GameData data)
    {
        string json = JsonUtility.ToJson(data);
        string path = persistentPath + "/" + data.Filename;

        if (!File.Exists(path)) File.Create(path).Close();

        File.WriteAllText(path, json);
    }

    public static void LoadFromJSON<T>(ref T data) where T : GameData
    {
        string path = Path.Combine(persistentPath, data.Filename);

        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        JsonUtility.FromJsonOverwrite(json, data);
    }
}

