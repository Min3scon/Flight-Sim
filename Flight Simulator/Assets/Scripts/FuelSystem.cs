using UnityEngine;
using TMPro;

public class FuelSystem : MonoBehaviour
{
    public float currentFuel;
    [SerializeField] private float maxFuel = 100f;
    [SerializeField] private float fuelConsumptionRate = 0.1f;
    [SerializeField] private PlaneController planeController;
    [SerializeField] private FuelBar fuelBar;

    public TMP_Text FuelHud;

    public float MaxFuel => maxFuel;

    void Start()
    {
        currentFuel = maxFuel;
        UpdateFuelDisplay();
    }

    void Update()
    {
        if (planeController == null || !planeController.enabled) return;

        float distance = planeController.currentSpeed * Time.deltaTime;
        currentFuel -= distance * fuelConsumptionRate;
        if (currentFuel <= 0)
        {
            currentFuel = 0;
            planeController.currentSpeed = 0f;
        }

        UpdateFuelDisplay();
    }

    void UpdateFuelDisplay()
    {
        if (fuelBar != null)
        {
            fuelBar.UpdateFuelBar(maxFuel, currentFuel);
        }

        if (FuelHud != null)
        {
            float fuelPercent = (currentFuel / maxFuel) * 100f;
            FuelHud.text = "Fuel: " + fuelPercent.ToString("F0") + "%";
        }
    }
}