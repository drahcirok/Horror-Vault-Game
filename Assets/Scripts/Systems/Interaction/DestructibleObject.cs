// Script: DestructibleObject.cs
using UnityEngine;

public class DestructibleObject : MonoBehaviour
{
    [SerializeField] private GameObject destroyedVersion; // Opcional: Versión en pedazos
    [SerializeField] private float destroyDelay = 0f;

    public void Shatter()
    {
        // Aquí podrías poner sonido de madera rompiéndose
        
        if (destroyedVersion != null)
        {
            // Aparecer los escombros en la misma posición
            Instantiate(destroyedVersion, transform.position, transform.rotation);
        }

        // Destruir este objeto
        Destroy(gameObject, destroyDelay);
    }
}