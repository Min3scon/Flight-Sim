using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FuelBar : MonoBehaviour
{
    [SerializeField] private Image fuelBarSprite;
    [SerializeField] private TextMeshProUGUI fuelAmountText;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    public void UpdateFuelBar(float maxFuel, float currentFuel)
    {
        if (fuelBarSprite == null) return;
        
        float fillAmount = currentFuel / maxFuel;
        fuelBarSprite.fillAmount = fillAmount;

        if (fuelAmountText != null)
        {
            float percentage = (currentFuel / maxFuel) * 100f;
            fuelAmountText.text = percentage.ToString("F0") + "%";
        }
    }

    void Update()
    {
        if (cam == null) return;
        
        transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}