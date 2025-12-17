// Script: RichardPlayer.cs
// Ubicación: Assets/Scripts/Characters/
using UnityEngine;

public class RichardPlayer : PlayerBase
{
    [Header("Richard's Tech Ability")]
    [SerializeField] private GameObject hackingEffect;
    [SerializeField] private float hackRange = 10f;
    
    void Start()
    {
        characterType = CharacterType.Richard;
        characterName = "Richard";
        Initialize();
    }

    void Update()
    {
        if (!IsActive) return;
        
        HandleMovement();
        
        // Hackear con Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecialAbility();
        }
    }

    public override void ActivateSpecialAbility()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, hackRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                Debug.Log($"Hackeando: {hit.collider.name}");
                // Lógica específica de hackeo
                if (hackingEffect != null)
                {
                    Instantiate(hackingEffect, hit.point, Quaternion.identity);
                }
            }
        }
    }

    public override string GetAbilityDescription()
    {
        return "HACKEO TECNOLÓGICO: Interactúa con sistemas y puertas electrónicas";
    }
}