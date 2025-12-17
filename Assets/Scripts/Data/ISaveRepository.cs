// Script: ISaveRepository.cs
// Ubicación: Assets/Scripts/Data/
public interface ISaveRepository
{
    /// <summary>
    /// Guarda los datos del jugador
    /// </summary>
    bool SaveGame(PlayerData data, string saveSlot = "default");
    
    /// <summary>
    /// Carga los datos del jugador
    /// </summary>
    PlayerData LoadGame(string saveSlot = "default");
    
    /// <summary>
    /// Verifica si existe una partida guardada
    /// </summary>
    bool SaveExists(string saveSlot = "default");
    
    /// <summary>
    /// Elimina una partida guardada
    /// </summary>
    bool DeleteSave(string saveSlot = "default");
    
    /// <summary>
    /// Obtiene todas las partidas guardadas
    /// </summary>
    string[] GetAllSaveSlots();
}