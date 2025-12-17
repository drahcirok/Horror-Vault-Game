// Script: IInteractable.cs
// Ubicación: Assets/Scripts/Interfaces/
public interface IInteractable
{
    /// <summary>
    /// Método principal de interacción
    /// </summary>
    void Interact(PlayerBase player);
    
    /// <summary>
    /// Mensaje que aparece en UI cuando el jugador mira el objeto
    /// </summary>
    string GetInteractionPrompt();
    
    /// <summary>
    /// Verifica si el personaje puede interactuar con esto
    /// </summary>
    bool CanInteract(PlayerBase player);
    
    /// <summary>
    /// Distancia máxima para interactuar
    /// </summary>
    float GetInteractionRange();
}