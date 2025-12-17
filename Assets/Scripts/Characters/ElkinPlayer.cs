// Script: ElkinPlayer.cs (VERSIÓN FPS)
using UnityEngine;

public class ElkinPlayer : PlayerBase
{
    [Header("Estadísticas de Fuerza")]
    [SerializeField] private float pushForce = 50f;
    [SerializeField] private float hitRange = 4f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private GameObject hitEffect;

    void Start()
    {
        characterType = CharacterType.Elkin;
        characterName = "Elkin";
        Initialize(); // Esto llamará a la lógica de pegar la cámara
    }

    void Update()
    {
        if (!IsActive) return;

        HandleMovement(); // Esto maneja WASD y Mouse

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ActivateSpecialAbility();
        }
    }

    public override void ActivateSpecialAbility()
    {
        // FPS: El rayo sale de la cámara hacia donde miras
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        // Debug: verás la línea roja saliendo de tus ojos
        Debug.DrawRay(ray.origin, ray.direction * hitRange, Color.red, 2f);

        if (Physics.Raycast(ray, out hit, hitRange, obstacleLayer))
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Empujar en la dirección de la cámara
                rb.AddForce(playerCamera.transform.forward * pushForce, ForceMode.Impulse);
                Debug.Log("¡PUM! Objeto empujado");

                if (hitEffect != null)
                    Instantiate(hitEffect, hit.point, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("Golpe al aire (Apuna mejor a la Roca)");
        }
    }

    public override string GetAbilityDescription()
    {
        return "FUERZA BRUTA: Apunta y presiona Q para empujar/romper.";
    }
}