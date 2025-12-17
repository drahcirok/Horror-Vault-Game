// Script: PlayerData.cs
// Ubicación: Assets/Scripts/Data/
using System;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    // Datos del progreso
    public int currentLevel = 1;
    public int totalScore = 0;
    public DateTime lastSaveTime;
    
    // Datos de posición
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    
    // Estado del juego
    public CharacterType lastUsedCharacter;
    public bool[] unlockedDoors;  // IDs de puertas desbloqueadas
    public bool[] collectedItems; // IDs de ítems coleccionados
    
    // Estadísticas
    public int enemiesDefeated;
    public float playTimeInSeconds;
    public int deathsCount;
    
    // Constructor
    public PlayerData()
    {
        lastSaveTime = DateTime.Now;
        unlockedDoors = new bool[50]; // Espacio para 50 puertas
        collectedItems = new bool[100]; // Espacio para 100 ítems
    }
    
    // Métodos helper
    public Vector3 GetPlayerPosition()
    {
        return new Vector3(playerPositionX, playerPositionY, playerPositionZ);
    }
    
    public void SetPlayerPosition(Vector3 position)
    {
        playerPositionX = position.x;
        playerPositionY = position.y;
        playerPositionZ = position.z;
    }
}