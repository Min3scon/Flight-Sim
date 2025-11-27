using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public CharacterController controller;
    
    [Header("Settings")]
    public float mouseSensitivity = 2f;
    public float moveSpeed = 6f;
    public float jumpHeight = 1.5f;
    public float gravity = 9.81f;
    
    [Header("Ground Check")]
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask = -1;
    
    float cameraVerticalRotation = 0f;
    Vector3 velocity;
    bool isGrounded;
    
    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = GetComponentInChildren<Camera>().transform;
        
        if (controller == null)
            controller = GetComponent<CharacterController>();
        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + controller.height / 2f, groundMask);
        
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        transform.Rotate(Vector3.up * mouseX);
        
        cameraVerticalRotation -= mouseY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        cameraTransform.localEulerAngles = Vector3.right * cameraVerticalRotation;
        
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        
        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * moveSpeed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * 2f * gravity);
        }
        
        velocity.y -= gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    
    void OnDrawGizmosSelected()
    {

        if (controller != null)
        {
            Gizmos.color = Color.red;
            Vector3 start = transform.position;
            Vector3 end = start + Vector3.down * (groundCheckDistance + controller.height / 2f);
            Gizmos.DrawLine(start, end);
        }
    }
}