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

    [Header("Audio (engine only)")]
    [SerializeField] AudioClip engineClip;
    [SerializeField] AudioSource engineSource; 
    [SerializeField] float minPitch = 0.8f;
    [SerializeField] float maxPitch = 1.6f;
    [SerializeField, Range(0f, 1f)] float minVolume = 0.2f;
    [SerializeField, Range(0f, 1f)] float maxVolume = 1f;

    public float currentSpeed = 0f;
    float yaw;

    void Start()
    {
        if (engineSource == null)
        {
            var go = new GameObject("PlaneEngineAudio");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.zero;
            engineSource = go.AddComponent<AudioSource>();
        }

        engineSource.playOnAwake = false;
        engineSource.loop = true;
        engineSource.clip = engineClip;
        engineSource.spatialBlend = 1f;
        engineSource.rolloffMode = AudioRolloffMode.Logarithmic;
        engineSource.minDistance = 5f;
        engineSource.maxDistance = 500f;

        if (engineClip != null) engineSource.Play();
    }

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

        yaw += horizontalInput * YawAmount * Time.deltaTime;

        float pitch = -verticalInput * PitchAmount;
        float roll = -horizontalInput * RollAmount;

        transform.rotation = Quaternion.Euler(pitch, yaw, roll);
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

        UpdateEngineAudio(throttle);
    }

    void UpdateEngineAudio(bool throttle)
    {
        if (engineSource == null || engineClip == null) return;

        float t = MaxSpeed > 0f ? Mathf.Clamp01(currentSpeed / MaxSpeed) : 0f;
        engineSource.pitch = Mathf.Lerp(minPitch, maxPitch, t);
        engineSource.volume = Mathf.Lerp(minVolume, maxVolume, throttle ? Mathf.Max(t, 0.3f) : t);

        if (!engineSource.isPlaying) engineSource.Play();
    }
}