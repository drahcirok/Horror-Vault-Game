// Script: JsonSaveRepository.cs
// Ubicación: Assets/Scripts/Data/
using System;
using System.IO;
using UnityEngine;

public class JsonSaveRepository : ISaveRepository
{
    private readonly string saveDirectory;
    
    public JsonSaveRepository()
    {
        // Usa Application.persistentDataPath para que funcione en todas las plataformas
        saveDirectory = Path.Combine(Application.persistentDataPath, "Saves");
        
        // Crear directorio si no existe
        if (!Directory.Exists(saveDirectory))
        {
            Directory.CreateDirectory(saveDirectory);
            Debug.Log($"Directorio de guardado creado: {saveDirectory}");
        }
    }
    
    public bool SaveGame(PlayerData data, string saveSlot = "default")
    {
        try
        {
            // Actualizar tiempo de guardado
            data.lastSaveTime = DateTime.Now;
            
            // Convertir a JSON
            string jsonData = JsonUtility.ToJson(data, true);
            
            // Ruta del archivo
            string filePath = GetSaveFilePath(saveSlot);
            
            // Escribir archivo
            File.WriteAllText(filePath, jsonData);
            
            Debug.Log($"Partida guardada en: {filePath}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al guardar partida: {e.Message}");
            return false;
        }
    }
    
    public PlayerData LoadGame(string saveSlot = "default")
    {
        try
        {
            string filePath = GetSaveFilePath(saveSlot);
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"No se encontró partida guardada: {saveSlot}");
                return null;
            }
            
            // Leer archivo
            string jsonData = File.ReadAllText(filePath);
            
            // Convertir desde JSON
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            
            Debug.Log($"Partida cargada desde: {filePath}");
            return data;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al cargar partida: {e.Message}");
            return null;
        }
    }
    
    public bool SaveExists(string saveSlot = "default")
    {
        string filePath = GetSaveFilePath(saveSlot);
        return File.Exists(filePath);
    }
    
    public bool DeleteSave(string saveSlot = "default")
    {
        try
        {
            string filePath = GetSaveFilePath(saveSlot);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                Debug.Log($"Partida eliminada: {saveSlot}");
                return true;
            }
            
            return false;
        }
        catch (Exception e)
        {
            Debug.LogError($"Error al eliminar partida: {e.Message}");
            return false;
        }
    }
    
    public string[] GetAllSaveSlots()
    {
        if (!Directory.Exists(saveDirectory))
            return new string[0];
            
        string[] files = Directory.GetFiles(saveDirectory, "*.json");
        string[] saveSlots = new string[files.Length];
        
        for (int i = 0; i < files.Length; i++)
        {
            saveSlots[i] = Path.GetFileNameWithoutExtension(files[i]);
        }
        
        return saveSlots;
    }
    
    private string GetSaveFilePath(string saveSlot)
    {
        // Asegurar que el nombre del archivo sea seguro
        string safeFileName = Path.GetFileName(saveSlot);
        safeFileName = safeFileName.Replace("..", "").Replace("/", "").Replace("\\", "");
        
        return Path.Combine(saveDirectory, $"{safeFileName}.json");
    }
}