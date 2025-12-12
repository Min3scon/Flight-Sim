using UnityEngine;
using DefaultNamespace;

public class FuelPickup : MonoBehaviour, IInteractable
{
    public FuelSystem planeFuelSystem;
    public FuelBar fuelBar;
    public Transform holdPosition;
    public float refuelRange = 3f;

    bool equipped = false;
    Rigidbody rb;
    Collider coll;
    Vector3 originalScale;
    Camera playerCamera;

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
        if (!equipped) return;

        transform.position = holdPosition.position;
        transform.rotation = holdPosition.rotation;

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Drop();
        }

        bool isRefueling = false;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out RaycastHit hit, refuelRange))
        {
            FuelSystem hitFuelSystem = hit.collider.GetComponentInParent<FuelSystem>();
            if (hitFuelSystem == planeFuelSystem && Input.GetMouseButton(0))
            {
                isRefueling = true;
                planeFuelSystem.StartFueling();
            }
        }

        if (!isRefueling)
        {
            planeFuelSystem.StopFueling();
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
        planeFuelSystem.StopFueling();
        rb.isKinematic = false;
        coll.enabled = true;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(playerCamera.transform.forward * 3f + Vector3.up * 2f, ForceMode.Impulse);
    }
}