// Script: AbilityDoor.cs
// Ubicación: Assets/Scripts/Systems/Interaction/
using UnityEngine;

public class AbilityDoor : MonoBehaviour, IInteractable
{
    [Header("Door Requirements")]
    [SerializeField] private CharacterType requiredCharacter;
    [SerializeField] private string doorName = "Puerta Especial";
    
    [Header("Visual Feedback")]
    [SerializeField] private Material lockedMaterial;
    [SerializeField] private Material unlockedMaterial;
    [SerializeField] private GameObject lockedEffect;
    [SerializeField] private GameObject unlockedEffect;
    
    [Header("Door Settings")]
    [SerializeField] private float interactionRange = 3f;
    [SerializeField] private float openSpeed = 2f;
    [SerializeField] private float openHeight = 3f;
    
    private bool isOpen = false;
    private bool isLocked = true;
    private Vector3 closedPosition;
    private MeshRenderer meshRenderer;
    
    void Start()
    {
        closedPosition = transform.position;
        meshRenderer = GetComponent<MeshRenderer>();
        
        // Aplicar material de bloqueado
        if (meshRenderer != null && lockedMaterial != null)
        {
            meshRenderer.material = lockedMaterial;
        }
        
        if (lockedEffect != null)
        {
            lockedEffect.SetActive(true);
        }
    }
    
    void Update()
    {
        if (isOpen)
        {
            // Animación de apertura
            transform.position = Vector3.Lerp(
                transform.position, 
                closedPosition + Vector3.up * openHeight, 
                Time.deltaTime * openSpeed
            );
        }
    }
    
    public void Interact(PlayerBase player)
    {
        if (!CanInteract(player))
        {
            // Feedback de error
            Debug.Log($"¡Necesitas a {requiredCharacter} para abrir esta puerta!");
            
            // Mostrar mensaje en UI (si tienes sistema de UI)
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowMessage(
                    $"Requieres a {requiredCharacter}",
                    Color.red,
                    2f
                );
            }
            return;
        }
        
        // Desbloquear y abrir
        isLocked = false;
        isOpen = true;
        
        // Cambiar material
        if (meshRenderer != null && unlockedMaterial != null)
        {
            meshRenderer.material = unlockedMaterial;
        }
        
        // Efectos visuales
        if (lockedEffect != null) lockedEffect.SetActive(false);
        if (unlockedEffect != null)
        {
            unlockedEffect.SetActive(true);
            Destroy(unlockedEffect, 3f);
        }
        
        Debug.Log($"¡{doorName} abierta por {player.characterName}!");
    }
    
    public string GetInteractionPrompt()
    {
        if (isLocked)
        {
            return $"Presiona E para abrir\n[Requiere: {requiredCharacter}]";
        }
        return isOpen ? "Abierta" : "Presiona E para abrir";
    }
    
    public bool CanInteract(PlayerBase player)
    {
        // Solo el personaje requerido puede abrir
        return player.characterType == requiredCharacter && !isOpen;
    }
    
    public float GetInteractionRange()
    {
        return interactionRange;
    }
    
    // Para debug
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}