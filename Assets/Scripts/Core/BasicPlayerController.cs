using UnityEngine;

public class BasicPlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    void Update()
    {
        // MOVIMIENTO BÁSICO WASD
        float horizontal = Input.GetAxis("Horizontal"); // A / D
        float vertical = Input.GetAxis("Vertical");     // W / S
        
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement = movement.normalized * moveSpeed * Time.deltaTime;
        
        transform.Translate(movement);
        
        // DEBUG: Mostrar teclas presionadas
        if (Input.GetKeyDown(KeyCode.W)) Debug.Log("W presionado");
        if (Input.GetKeyDown(KeyCode.Space)) Debug.Log("Espacio presionado");
    }
}