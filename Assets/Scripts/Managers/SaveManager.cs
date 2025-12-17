// Script: SaveManager.cs
// Ubicación: Assets/Scripts/Managers/
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }
    
    private ISaveRepository saveRepository;
    private PlayerData currentPlayerData;
    
    [Header("Auto-save Settings")]
    [SerializeField] private bool enableAutoSave = true;
    [SerializeField] private float autoSaveInterval = 60f; // Segundos
    
    private float autoSaveTimer = 0f;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Inicializar repositorio (puedes cambiar entre JSON, Binary, PlayerPrefs, etc.)
        saveRepository = new JsonSaveRepository();
        
        // Cargar datos o crear nuevos
        LoadOrCreateNewGame();
    }
    
    void Update()
    {
        // Auto-save
        if (enableAutoSave)
        {
            autoSaveTimer += Time.deltaTime;
            if (autoSaveTimer >= autoSaveInterval)
            {
                AutoSave();
                autoSaveTimer = 0f;
            }
        }
        
        // Guardado rápido con F5
        if (Input.GetKeyDown(KeyCode.F5))
        {
            QuickSave();
        }
        
        // Carga rápida con F9
        if (Input.GetKeyDown(KeyCode.F9))
        {
            QuickLoad();
        }
    }
    
    // ===== MÉTODOS PÚBLICOS =====
    
    public void SaveGame(string saveSlot = "default")
    {
        if (currentPlayerData == null)
        {
            currentPlayerData = new PlayerData();
        }
        
        // Actualizar datos antes de guardar
        UpdatePlayerData();
        
        bool success = saveRepository.SaveGame(currentPlayerData, saveSlot);
        
        if (success)
        {
            Debug.Log($"Partida guardada exitosamente en slot: {saveSlot}");
            // Aquí podrías mostrar un mensaje en UI
        }
    }
    
    public void LoadGame(string saveSlot = "default")
    {
        PlayerData loadedData = saveRepository.LoadGame(saveSlot);
        
        if (loadedData != null)
        {
            currentPlayerData = loadedData;
            ApplyLoadedData();
            Debug.Log($"Partida cargada desde slot: {saveSlot}");
        }
        else
        {
            Debug.LogWarning($"No se pudo cargar la partida del slot: {saveSlot}");
        }
    }
    
    public void NewGame()
    {
        currentPlayerData = new PlayerData();
        ApplyLoadedData();
        Debug.Log("Nueva partida creada");
    }
    
    public void QuickSave()
    {
        SaveGame("quicksave");
    }
    
    public void QuickLoad()
    {
        if (saveRepository.SaveExists("quicksave"))
        {
            LoadGame("quicksave");
        }
        else
        {
            Debug.LogWarning("No hay partida rápida guardada");
        }
    }
    
    // ===== MÉTODOS PRIVADOS =====
    
    private void LoadOrCreateNewGame()
    {
        if (saveRepository.SaveExists("default"))
        {
            LoadGame("default");
        }
        else
        {
            NewGame();
        }
    }
    
    private void AutoSave()
    {
        if (enableAutoSave)
        {
            SaveGame("autosave");
        }
    }
    
    private void UpdatePlayerData()
    {
        // Aquí actualizas los datos del jugador actual
        if (CharacterManager.Instance != null)
        {
            PlayerBase currentPlayer = CharacterManager.Instance.GetCurrentPlayer();
            if (currentPlayer != null)
            {
                currentPlayerData.SetPlayerPosition(currentPlayer.transform.position);
                currentPlayerData.lastUsedCharacter = currentPlayer.characterType;
            }
        }
        
        // Actualizar tiempo de juego
        currentPlayerData.playTimeInSeconds += Time.deltaTime;
    }
    
    private void ApplyLoadedData()
    {
        // Aquí aplicas los datos cargados al juego
        if (CharacterManager.Instance != null)
        {
            // Mover al jugador a la posición guardada
            PlayerBase currentPlayer = CharacterManager.Instance.GetCurrentPlayer();
            if (currentPlayer != null)
            {
                currentPlayer.transform.position = currentPlayerData.GetPlayerPosition();
            }
            
            // Cambiar al personaje usado por última vez
            CharacterManager.Instance.SwitchToCharacter(currentPlayerData.lastUsedCharacter);
        }
        
        // Aplicar otros datos del juego (puertas desbloquedas, etc.)
        Debug.Log($"Partida cargada - Nivel: {currentPlayerData.currentLevel}");
    }
    
    // ===== MÉTODOS PARA ACCESO A DATOS =====
    
    public PlayerData GetCurrentPlayerData()
    {
        return currentPlayerData;
    }
    
    public void UnlockDoor(int doorId)
    {
        if (currentPlayerData != null && doorId >= 0 && doorId < currentPlayerData.unlockedDoors.Length)
        {
            currentPlayerData.unlockedDoors[doorId] = true;
        }
    }
    
    public bool IsDoorUnlocked(int doorId)
    {
        if (currentPlayerData != null && doorId >= 0 && doorId < currentPlayerData.unlockedDoors.Length)
        {
            return currentPlayerData.unlockedDoors[doorId];
        }
        return false;
    }
}