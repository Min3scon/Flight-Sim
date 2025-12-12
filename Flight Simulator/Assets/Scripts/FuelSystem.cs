using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class FuelSystem : MonoBehaviour
{
    public float currentFuel;
    [SerializeField] float maxFuel = 100f;
    [SerializeField] float fuelConsumptionRate = 0.1f;
    [SerializeField] float fuelFillRate = 25f;
    [SerializeField] PlaneController planeController;
    [SerializeField] FuelBar fuelBar;
    [SerializeField] TMP_Text FuelHud;
    [SerializeField] AudioClip fuelingClip;

    AudioSource fuelingAudioSource;
    bool isFueling;

    public float MaxFuel => maxFuel;
    public bool IsFueling => isFueling;

    void Start()
    {
        fuelingAudioSource = GetComponent<AudioSource>();
        fuelingAudioSource.playOnAwake = false;
        fuelingAudioSource.loop = true;
        currentFuel = maxFuel;
        UpdateFuelDisplay();
    }

    void Update()
    {
        HandleFueling();
        HandleConsumption();
        UpdateFuelDisplay();
    }

    void HandleFueling()
    {
        if (!isFueling) return;

        if (currentFuel >= maxFuel)
        {
            currentFuel = maxFuel;
            StopFueling();
            return;
        }

        currentFuel += fuelFillRate * Time.deltaTime;
        if (currentFuel >= maxFuel)
        {
            currentFuel = maxFuel;
            StopFueling();
        }
    }

    void HandleConsumption()
    {
        if (planeController == null || !planeController.enabled) return;

        float distance = planeController.currentSpeed * Time.deltaTime;
        currentFuel -= distance * fuelConsumptionRate;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            planeController.currentSpeed = 0f;
        }
    }

    public void StartFueling()
    {
        if (currentFuel >= maxFuel)
        {
            StopFueling();
            return;
        }

        isFueling = true;
        if (fuelingAudioSource != null)
        {
            if (fuelingClip != null) fuelingAudioSource.clip = fuelingClip;
            if (!fuelingAudioSource.isPlaying) fuelingAudioSource.Play();
        }
    }

    public void StopFueling()
    {
        isFueling = false;
        if (fuelingAudioSource != null && fuelingAudioSource.isPlaying) fuelingAudioSource.Stop();
    }

    void OnDisable()
    {
        StopFueling();
    }

    void UpdateFuelDisplay()
    {
        if (fuelBar != null)
            fuelBar.UpdateFuelBar(maxFuel, currentFuel);

        if (FuelHud != null)
        {
            float fuelPercent = (currentFuel / maxFuel) * 100f;
            FuelHud.text = "Fuel: " + fuelPercent.ToString("F0") + "%";
        }
    }
}