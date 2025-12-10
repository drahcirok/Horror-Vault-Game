using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public GameObject[] characters;  // Los 4 cubos
    public Text uiText;              // Para mostrar nombre
    
    private int currentIndex = 0;
    
    void Start()
    {
        // Al inicio, solo mostrar el primer personaje
        SwitchToCharacter(0);
    }
    
    void Update()
    {
        // Cambiar con teclas 1-4
        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToCharacter(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToCharacter(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchToCharacter(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchToCharacter(3);
        
        // Cambiar con Tab
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int next = (currentIndex + 1) % characters.Length;
            SwitchToCharacter(next);
        }
    }
    
    void SwitchToCharacter(int index)
    {
        // Ocultar todos
        foreach (GameObject character in characters)
        {
            character.SetActive(false);
        }
        
        // Mostrar el seleccionado
        characters[index].SetActive(true);
        currentIndex = index;
        
        // Actualizar UI
        if (uiText != null)
            uiText.text = "Personaje: " + characters[index].name;
            
        Debug.Log("Cambiado a: " + characters[index].name);
    }
}