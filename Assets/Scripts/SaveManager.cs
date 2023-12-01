using System.IO;
using UnityEngine;

public static class SaveManager
{
    public static void SaveData(string json, string path)
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);
        File.WriteAllText(filePath, json);
    }

    public static string LoadData(string path)
    {
        string filePath = Path.Combine(Application.persistentDataPath, path);

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }
    }
}
