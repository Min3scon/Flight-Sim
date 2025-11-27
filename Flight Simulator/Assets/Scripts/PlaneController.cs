using UnityEngine;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float MaxSpeed = 20f;
    public float Acceleration = 10f;
    public float YawAmount = 120f;
    public float PitchAmount = 20f;
    public float RollAmount = 30f;

    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI altitudeText;

    public float currentSpeed = 0f;
    private float Yaw;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        bool throttle = Input.GetKey(KeyCode.Space);

        if (throttle)
        {
            currentSpeed += Acceleration * Time.deltaTime;
            if (currentSpeed > MaxSpeed) currentSpeed = MaxSpeed;
        }
        else
        {
            currentSpeed -= Acceleration * Time.deltaTime;
            if (currentSpeed < 0f) currentSpeed = 0f;
        }

        Yaw += horizontalInput * YawAmount * Time.deltaTime;

        float pitch = -verticalInput * PitchAmount;
        float roll = -horizontalInput * RollAmount;

        transform.rotation = Quaternion.Euler(pitch, Yaw, roll);

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        if (speedText != null)
        {
            float mph = currentSpeed * 2.23694f;
            speedText.text = "Speed: " + Mathf.RoundToInt(mph) + " mph";
        }

        if (altitudeText != null)
        {
            altitudeText.text = "Height: " + Mathf.RoundToInt(transform.position.y) + " M";
        }
    }
}