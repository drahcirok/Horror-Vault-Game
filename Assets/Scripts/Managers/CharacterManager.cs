// Script: CharacterManager.cs (VERSIÓN ACTUALIZADA)
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }
    
    [Header("Character Prefabs")]
    [SerializeField] private PlayerBase[] characterPrefabs;
    
    [Header("UI References")]
    [SerializeField] private Text characterNameText;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Text abilityDescriptionText;
    
    private PlayerBase[] characters;
    private int currentCharacterIndex = 0;
    
    // Propiedad para acceso fácil
    public PlayerBase CurrentPlayer 
    { 
        get 
        { 
            return characters != null && currentCharacterIndex >= 0 && currentCharacterIndex < characters.Length 
                ? characters[currentCharacterIndex] 
                : null; 
        } 
    }
    
    void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    void Start()
    {
        InitializeCharacters();
        
        // Empezar con el primer personaje
        if (characters.Length > 0)
        {
            SwitchToCharacter(0);
        }
    }
    
    void Update()
    {
        HandleCharacterSwitch();
        
        // Actualizar UI si hay cambios
        if (CurrentPlayer != null)
        {
            UpdateUI();
        }
    }
    
    private void InitializeCharacters()
    {
        characters = new PlayerBase[characterPrefabs.Length];
        
        // Instanciar todos los personajes
        for (int i = 0; i < characterPrefabs.Length; i++)
        {
            if (characterPrefabs[i] != null)
            {
                PlayerBase characterInstance = Instantiate(
                    characterPrefabs[i], 
                    Vector3.zero, 
                    Quaternion.identity
                );
                
                characterInstance.gameObject.name = characterPrefabs[i].characterName;
                characterInstance.gameObject.SetActive(false);
                characters[i] = characterInstance;
                
                Debug.Log($"Personaje inicializado: {characterInstance.characterName}");
            }
        }
    }
    
    private void HandleCharacterSwitch()
    {
        // Cambiar con teclas 1-4
        for (int i = 0; i < 4 && i < characters.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SwitchToCharacter(i);
                return;
            }
        }
        
        // Cambiar con Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int nextIndex = (currentCharacterIndex + 1) % characters.Length;
            SwitchToCharacter(nextIndex);
        }
    }
    
    public void SwitchToCharacter(int newIndex)
    {
        // Validaciones
        if (characters == null || characters.Length == 0)
        {
            Debug.LogError("No hay personajes inicializados");
            return;
        }
        
        if (newIndex < 0 || newIndex >= characters.Length)
        {
            Debug.LogError($"Índice de personaje inválido: {newIndex}");
            return;
        }
        
        if (characters[newIndex] == null)
        {
            Debug.LogError($"Personaje en índice {newIndex} es null");
            return;
        }
        
        // Desactivar personaje actual si existe
        if (CurrentPlayer != null)
        {
            Vector3 currentPosition = CurrentPlayer.transform.position;
            Quaternion currentRotation = CurrentPlayer.transform.rotation;
            
            CurrentPlayer.gameObject.SetActive(false);
            
            // Posicionar nuevo personaje en la misma posición
            characters[newIndex].transform.position = currentPosition;
            characters[newIndex].transform.rotation = currentRotation;
        }
        
        // Activar nuevo personaje
        currentCharacterIndex = newIndex;
        characters[newIndex].gameObject.SetActive(true);
        
        // Notificar al GameManager si es necesario
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ShowMessage(
                $"Cambiado a: {CurrentPlayer.characterName}",
                CurrentPlayer.characterColor,
                1.5f
            );
        }
        
        Debug.Log($"Personaje cambiado a: {CurrentPlayer.characterName}");
        
        // Actualizar UI
        UpdateUI();
    }
    
    public void SwitchToCharacter(CharacterType characterType)
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] != null && characters[i].characterType == characterType)
            {
                SwitchToCharacter(i);
                return;
            }
        }
        
        Debug.LogWarning($"No se encontró personaje de tipo: {characterType}");
    }
    
    private void UpdateUI()
    {
        if (characterNameText != null)
            characterNameText.text = CurrentPlayer.characterName;
        
        if (characterIcon != null)
            characterIcon.color = CurrentPlayer.characterColor;
        
        if (abilityDescriptionText != null)
            abilityDescriptionText.text = CurrentPlayer.GetAbilityDescription();
    }
    
    // Método helper para obtener personaje por tipo
    public PlayerBase GetCharacterByType(CharacterType type)
    {
        foreach (PlayerBase character in characters)
        {
            if (character != null && character.characterType == type)
            {
                return character;
            }
        }
        return null;
    }
    // ===== SOLUCIÓN DEL ERROR CS1061 =====
    public PlayerBase GetCurrentPlayer()
    {
        return CurrentPlayer;
    }
}