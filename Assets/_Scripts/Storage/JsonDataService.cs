using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public static class JsonDataService
{
    public static void SaveData<T>(T data, string filePath, bool prettyPrint = true)
    {
        filePath = Path.Combine(Application.persistentDataPath, filePath);

        try
        {
            var formatting = prettyPrint ? Formatting.Indented : Formatting.None;
            string json = JsonConvert.SerializeObject(data, formatting);

            File.WriteAllText(filePath, json);
#if UNITY_EDITOR
            //Debug.Log($"[JsonDataService] Saved to: {filePath}");
#endif
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[JsonDataService] Failed to save data: {ex.Message}");
        }
    }

    public static void LoadDataTo<T>(T target, string filePath)
    {
        filePath = Path.Combine(Application.persistentDataPath, filePath);

        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[JsonDataService] File not found: {filePath}");
                SaveData<T>(target, filePath);
                return;
            }

            string json = File.ReadAllText(filePath);
            JsonConvert.PopulateObject(json, target);
#if UNITY_EDITOR
            //Debug.Log($"[JsonDataService] Loaded from: {filePath}");
#endif
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[JsonDataService] Failed to load data: {ex.Message}");
            return;
        }
    }
    
    public static T GetLoadedData<T>(string filePath) where T : new()
    {
        filePath = Path.Combine(Application.persistentDataPath, filePath);

        try
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"[JsonDataService] File not found: {filePath}");
                T defaultData = new T();
                SaveData<T>(defaultData, filePath);
                return defaultData;
            }

            string json = File.ReadAllText(filePath);
            T data = JsonConvert.DeserializeObject<T>(json);
#if UNITY_EDITOR
            //Debug.Log($"[JsonDataService] Loaded from: {filePath}");
#endif
            return data;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"[JsonDataService] Failed to load data: {ex.Message}");
            return default;
        }
    }
}