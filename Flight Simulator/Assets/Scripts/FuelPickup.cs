using UnityEngine;
using DefaultNamespace;

public class FuelPickup : MonoBehaviour, IInteractable
{
    public FuelSystem planeFuelSystem;
    public FuelBar fuelBar;
    public Transform holdPosition;
    public float fuelRate = 10f;
    public float refuelRange = 3f;

    private bool equipped = false;
    private Rigidbody rb;
    private Collider coll;
    private Vector3 originalScale;
    private Camera playerCamera;

    public string InteractMessage => equipped ? "Press E to Drop" : "Press E to Pick Up";

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        originalScale = transform.localScale;
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (equipped)
        {
            transform.position = holdPosition.position;
            transform.rotation = holdPosition.rotation;

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Drop();
            }

            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, refuelRange))
            {
                FuelSystem hitFuelSystem = hit.collider.GetComponentInParent<FuelSystem>();
                if (hitFuelSystem == planeFuelSystem && Input.GetMouseButton(0))
                {
                    planeFuelSystem.currentFuel += fuelRate * Time.deltaTime;
                    if (planeFuelSystem.currentFuel > planeFuelSystem.MaxFuel)
                        planeFuelSystem.currentFuel = planeFuelSystem.MaxFuel;

                    if (fuelBar != null)
                        fuelBar.UpdateFuelBar(planeFuelSystem.MaxFuel, planeFuelSystem.currentFuel);
                }
            }
        }
    }

    public void Interact()
    {
        if (!equipped)
            PickUp();
        else
            Drop();
    }

    void PickUp()
    {
        equipped = true;
        rb.isKinematic = true;
        coll.enabled = false;
    }

    void Drop()
    {
        equipped = false;
        rb.isKinematic = false;
        coll.enabled = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(playerCamera.transform.forward * 3f + Vector3.up * 2f, ForceMode.Impulse);
    }
}