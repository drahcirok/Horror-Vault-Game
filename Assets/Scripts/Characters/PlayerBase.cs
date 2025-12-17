// Script: PlayerBase.cs (CON AJUSTE DE ALTURA)
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    [Header("Character Identity")]
    public CharacterType characterType;
    public string characterName;
    public Sprite faceIcon;
    public Color characterColor;

    [Header("Movement Stats")]
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float mouseSensitivity = 2f;
    [SerializeField] protected float eyeHeight = 0.8f; // <--- NUEVA VARIABLE (Súbela en el Inspector)

    [Header("References")]
    protected Camera playerCamera;
    protected CharacterController controller;
    
    private float xRotation = 0f;

    public bool IsActive { get; protected set; }

    public abstract void ActivateSpecialAbility();
    public abstract string GetAbilityDescription();

    public virtual void Initialize()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected virtual void OnEnable()
    {
        IsActive = true;
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
            if (playerCamera == null) 
            {
                GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
                if (camObj != null) playerCamera = camObj.GetComponent<Camera>();
            }
        }
        
        if (playerCamera != null)
        {
            playerCamera.gameObject.SetActive(true);
            playerCamera.transform.SetParent(null); 
        }
    }

    protected virtual void OnDisable()
    {
        IsActive = false;
    }

    public virtual void HandleMovement()
    {
        if (!IsActive) return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        move.y = -9.81f * Time.deltaTime;

        if (controller != null)
        {
            controller.Move(move * moveSpeed * Time.deltaTime);
        }
    }

    protected virtual void LateUpdate()
    {
        if (IsActive && playerCamera != null)
        {
            // Usamos la variable eyeHeight en lugar del número fijo
            Vector3 eyePosition = transform.position + new Vector3(0f, eyeHeight, 0f);
            playerCamera.transform.position = eyePosition;

            playerCamera.transform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);
        }
    }
    
    public virtual bool CanInteractWith(Vector3 targetPosition)
    {
        return Vector3.Distance(transform.position, targetPosition) <= 3f;
    }
}