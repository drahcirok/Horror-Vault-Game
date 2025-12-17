// Script: PlayerInteraction.cs
// Ubicación: Assets/Scripts/Systems/Interaction/
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Raycast Settings")]
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private LayerMask interactableLayer;
    
    [Header("UI References")]
    [SerializeField] private Text interactionPromptText;
    [SerializeField] private GameObject interactionPromptPanel;
    
    private Camera playerCamera;
    private IInteractable currentInteractable;
    
    void Start()
    {
        playerCamera = Camera.main;
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
        }
    }
    
    void Update()
    {
        CheckForInteractables();
        
        // Interactuar con E
        if (Input.GetKeyDown(KeyCode.E) && currentInteractable != null)
        {
            PlayerBase currentPlayer = CharacterManager.Instance?.GetCurrentPlayer();
            if (currentPlayer != null)
            {
                currentInteractable.Interact(currentPlayer);
            }
        }
    }
    
    private void CheckForInteractables()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, raycastDistance, interactableLayer))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            
            if (interactable != null)
            {
                PlayerBase currentPlayer = CharacterManager.Instance?.GetCurrentPlayer();
                
                if (currentPlayer != null && interactable.CanInteract(currentPlayer))
                {
                    currentInteractable = interactable;
                    ShowPrompt(interactable.GetInteractionPrompt());
                    return;
                }
            }
        }
        
        // No hay interactable o no se puede interactuar
        currentInteractable = null;
        HidePrompt();
    }
    
    private void ShowPrompt(string prompt)
    {
        if (interactionPromptText != null)
        {
            interactionPromptText.text = prompt;
        }
        
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(true);
        }
    }
    
    private void HidePrompt()
    {
        if (interactionPromptPanel != null)
        {
            interactionPromptPanel.SetActive(false);
        }
    }
    
    // Debug visual
    void OnDrawGizmos()
    {
        if (playerCamera == null) return;
        
        Gizmos.color = Color.green;
        Vector3 rayStart = playerCamera.transform.position;
        Vector3 rayEnd = rayStart + playerCamera.transform.forward * raycastDistance;
        Gizmos.DrawLine(rayStart, rayEnd);
        Gizmos.DrawSphere(rayEnd, 0.1f);
    }
}