// Script: GameManager.cs
// Ubicación: Assets/Scripts/Managers/
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Interacting,  // Cuando usa habilidad o habla con NPC
    GameOver,
    Loading
}

public class GameManager : MonoBehaviour
{
    // ===== SINGLETON PATTERN =====
    public static GameManager Instance { get; private set; }
    
    // ===== VARIABLES PÚBLICAS =====
    [Header("Game State")]
    [SerializeField] private GameState currentGameState = GameState.Playing;
    
    [Header("UI References")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private Text messageText;
    [SerializeField] private float messageDisplayTime = 3f;
    
    [Header("Game Settings")]
    [SerializeField] private bool cursorLockedInGame = true;
    [SerializeField] private float gameTimeScale = 1f;
    
    // ===== EVENTOS =====
    public delegate void GameStateChanged(GameState newState);
    public static event GameStateChanged OnGameStateChanged;
    
    // ===== PROPIEDADES =====
    public GameState CurrentGameState
    {
        get => currentGameState;
        private set
        {
            if (currentGameState != value)
            {
                currentGameState = value;
                OnGameStateChanged?.Invoke(value);
                UpdateGameState();
            }
        }
    }
    
    // ===== MÉTODOS UNITY =====
    void Awake()
    {
        // Implementación robusta del Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"Múltiples instancias de GameManager detectadas. Destruyendo: {gameObject.name}");
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"GameManager inicializado: {gameObject.name}");
    }
    
    void Start()
    {
        InitializeGame();
    }
    
    void Update()
    {
        HandleInput();
    }
    
    // ===== MÉTODOS DE INICIALIZACIÓN =====
    private void InitializeGame()
    {
        // Configurar cursor
        UpdateCursorState();
        
        // Configurar escala de tiempo
        Time.timeScale = gameTimeScale;
        
        // Ocultar menús
        if (pauseMenu != null) pauseMenu.SetActive(false);
        if (gameOverMenu != null) gameOverMenu.SetActive(false);
        
        Debug.Log("Juego inicializado");
    }
    
    private void UpdateGameState()
    {
        switch (CurrentGameState)
        {
            case GameState.Playing:
                Time.timeScale = gameTimeScale;
                if (pauseMenu != null) pauseMenu.SetActive(false);
                UpdateCursorState();
                break;
                
            case GameState.Paused:
                Time.timeScale = 0f;
                if (pauseMenu != null) pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
                
            case GameState.Interacting:
                Time.timeScale = 0.5f; // Slow motion durante interacciones
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
                
            case GameState.GameOver:
                Time.timeScale = 0f;
                if (gameOverMenu != null) gameOverMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;
        }
    }
    
    // ===== MÉTODOS DE CONTROL =====
    private void HandleInput()
    {
        // Pausar/Despausar con ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (CurrentGameState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
            }
            else if (CurrentGameState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
            }
        }
    }
    
    private void UpdateCursorState()
    {
        if (cursorLockedInGame && CurrentGameState == GameState.Playing)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    // ===== MÉTODOS PÚBLICOS =====
    
    public void SetGameState(GameState newState)
    {
        CurrentGameState = newState;
    }
    
    public void ShowMessage(string message, Color color, float duration = -1)
    {
        if (messageText == null)
        {
            Debug.LogWarning("No hay referencia a messageText en GameManager");
            return;
        }
        
        messageText.text = message;
        messageText.color = color;
        messageText.gameObject.SetActive(true);
        
        float displayTime = duration > 0 ? duration : messageDisplayTime;
        Invoke(nameof(HideMessage), displayTime);
    }
    
    public void ResumeGame()
    {
        SetGameState(GameState.Playing);
    }
    
    public void RestartGame()
    {
        // Reiniciar nivel
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        SetGameState(GameState.Playing);
    }
    
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
    
    // ===== MÉTODOS PRIVADOS =====
    private void HideMessage()
    {
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
        }
    }
    
    // ===== DEBUG =====
    void OnGUI()
    {
        // Mostrar estado actual en esquina (solo en desarrollo)
        #if UNITY_EDITOR
        GUIStyle style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;
        GUI.Label(new Rect(10, 10, 200, 30), $"Estado: {CurrentGameState}", style);
        #endif
    }
}