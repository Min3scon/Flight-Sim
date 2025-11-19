using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneController : MonoBehaviour
{
    [Header("Plane Stats")]
    public float throttleIncrement = 0.15f;
    public float maxThrust = 500f;
    public float responsiveness = 18f;
    [Range(0.01f, 0.1f)] public float inputDeadzone = 0.05f;
    public float liftMultiplier = 0.3f;

    private float throttle;
    private float pitch;
    private float yaw;
    private float roll;

    private float responseModifier
    { 
        get { return rb ? rb.mass / 100f * responsiveness : responsiveness; }
    }

    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("PlaneController: No Rigidbody found!");
            return;
        }
        rb.drag = 0.1f;
        rb.angularDrag = 3f;
        throttle = 0f;
        rb.velocity = Vector3.zero;
        Debug.Log("Plane initialized and stationary.");
    }

    private float ClampInput(float input)
    {
        return Mathf.Abs(input) > inputDeadzone ? input : 0f;
    }

    private void HandleInputs()
    {
        roll = ClampInput(Input.GetAxis("Roll"));
        pitch = ClampInput(Input.GetAxis("Pitch"));
        yaw = ClampInput(Input.GetAxis("Yaw"));

        if (Input.GetKey(KeyCode.Space))
        {
            throttle = Mathf.Clamp(throttle + throttleIncrement * Time.deltaTime, 0f, 1f);
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            throttle = Mathf.Clamp(throttle - throttleIncrement * Time.deltaTime, 0f, 1f);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
        {
            Debug.Log($"Pitch input detected - Pitch Value: {pitch:F2}");
        }
    }

    private void Update()
    {
        if (rb == null) return;
        HandleInputs();
    }

    private void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 thrust = transform.forward * maxThrust * throttle;
        rb.AddForce(thrust);

        float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
        if (forwardSpeed > 0f)
            rb.AddForce(transform.up * forwardSpeed * liftMultiplier);

        rb.AddTorque(transform.right * pitch * responseModifier);
        rb.AddTorque(transform.forward * roll * responseModifier);
        rb.AddTorque(Vector3.up * yaw * responseModifier);
    }
}
