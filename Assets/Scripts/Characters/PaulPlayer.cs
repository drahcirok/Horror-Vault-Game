// Script: PaulPlayer.cs
// Ubicación: Assets/Scripts/Characters/
using UnityEngine;

public class PaulPlayer : PlayerBase
{
    [Header("Paul's Distortion Ability")]
    [SerializeField] private GameObject distortionEffect;
    [SerializeField] private LayerMask hiddenLayer;
    
    private bool distortionActive = false;

    void Start()
    {
        characterType = CharacterType.Paul;
        characterName = "Paul";
        Initialize();
    }

    void Update()
    {
        if (!IsActive) return;
        
        HandleMovement();
        
        // Activar habilidad con Q
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecialAbility();
        }
    }

    public override void ActivateSpecialAbility()
    {
        distortionActive = !distortionActive;
        
        if (distortionEffect != null)
            distortionEffect.SetActive(distortionActive);

        // Cambiar capa de cámara para ver objetos ocultos
        if (playerCamera != null)
        {
            if (distortionActive)
            {
                playerCamera.cullingMask |= (1 << LayerMask.NameToLayer("Hidden"));
                Debug.Log("Distorsión ACTIVADA - Viendo objetos ocultos");
            }
            else
            {
                playerCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Hidden"));
                Debug.Log("Distorsión DESACTIVADA");
            }
        }
    }

    public override string GetAbilityDescription()
    {
        return "VISIÓN DE DISTORSIÓN: Revela objetos y caminos ocultos";
    }
}