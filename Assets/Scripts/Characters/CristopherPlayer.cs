// Script: CristopherPlayer.cs (LINTERNA SINCRONIZADA)
using UnityEngine;

public class CristopherPlayer : PlayerBase
{
    [Header("Habilidad Narrativa")]
    [SerializeField] private GameObject flashlightObj; 
    
    private bool isFlashlightOn = false;

    void Start()
    {
        characterType = CharacterType.Cristopher;
        characterName = "Cristopher";
        Initialize();

        if (flashlightObj != null) 
            flashlightObj.SetActive(false);
    }

    void Update()
    {
        if (!IsActive) return;

        HandleMovement();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecialAbility();
        }
    }

    // Sobrescribimos LateUpdate para añadir la lógica de la linterna
    protected override void LateUpdate()
    {
        // 1. Llamamos a la base para que la cámara se mueva primero
        base.LateUpdate(); 

        // 2. Si tenemos linterna y cámara, sincronizamos la rotación
        if (flashlightObj != null && playerCamera != null)
        {
            // Hacemos que la linterna mire EXACTAMENTE a donde mira la cámara
            flashlightObj.transform.rotation = playerCamera.transform.rotation;
            
            // Opcional: Si quieres que la luz salga de los ojos (estilo Doom), descomenta esto:
            // flashlightObj.transform.position = playerCamera.transform.position;
        }
    }

    public override void ActivateSpecialAbility()
    {
        isFlashlightOn = !isFlashlightOn; 

        if (flashlightObj != null)
        {
            flashlightObj.SetActive(isFlashlightOn);
        }
        
        Debug.Log(isFlashlightOn ? "Linterna ENCENDIDA" : "Linterna APAGADA");
    }

    public override string GetAbilityDescription()
    {
        return "LUZ NARRATIVA: Ilumina zonas oscuras.";
    }
}