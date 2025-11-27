using UnityEngine;
using TMPro;
using DefaultNamespace;

public class PlaneInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] Camera planeCamera;
    [SerializeField] PlaneController planeController;
    [SerializeField] GameObject player;
    [SerializeField] GameObject fuelBarCanvas;

    TextMeshProUGUI interactionText;
    TextMeshProUGUI speedText;
    TextMeshProUGUI altitudeText;
    TextMeshProUGUI fuelHud;
    TextMeshProUGUI progressText;

    MovementManager playerMovement;
    Camera playerCamera;
    InteractableController interactableController;
    bool isInside = false;

    public string InteractMessage => isInside ? "Press E to Exit" : "Press E to Enter";

    void Start()
    {
        playerMovement = player.GetComponent<MovementManager>();
        playerCamera = player.GetComponentInChildren<Camera>();
        interactableController = player.GetComponent<InteractableController>();
        planeCamera.gameObject.SetActive(false);
        planeController.enabled = false;
        FindUITexts();
        TogglePlaneHUD(false);
        Debug.Log($"PlaneInteractable Start - interactableController found: {interactableController != null}");
    }

    void FindUITexts()
    {
        interactionText = GameObject.Find("Interaction")?.GetComponent<TextMeshProUGUI>();
        speedText = GameObject.Find("speed")?.GetComponent<TextMeshProUGUI>();
        altitudeText = GameObject.Find("altitude")?.GetComponent<TextMeshProUGUI>();
        fuelHud = GameObject.Find("FuelText")?.GetComponent<TextMeshProUGUI>();
        progressText = GameObject.Find("checkpoints")?.GetComponent<TextMeshProUGUI>();
        Debug.Log($"FindUITexts - interactionText found: {interactionText != null}");
    }

    void TogglePlaneHUD(bool show)
    {
        if (speedText != null) speedText.gameObject.SetActive(show);
        if (altitudeText != null) altitudeText.gameObject.SetActive(show);
        if (fuelHud != null) fuelHud.gameObject.SetActive(show);
        if (progressText != null) progressText.gameObject.SetActive(show);
        if (interactionText != null) interactionText.gameObject.SetActive(!show);
    }

    void Update()
    {
        if (isInside && Input.GetKeyDown(KeyCode.E))
        {
            ExitPlane();
        }
    }

    public void Interact()
    {
        if (!isInside)
            EnterPlane();
        else
            ExitPlane();
    }

    void EnterPlane()
    {
        Debug.Log("EnterPlane called");
        if (interactionText != null)
        {
            Debug.Log($"Clearing interaction text. Current text: '{interactionText.text}'");
            interactionText.text = "";
            Debug.Log($"After clearing: '{interactionText.text}'");
        }
        else
        {
            Debug.LogWarning("interactionText is NULL!");
        }
        
        player.SetActive(false);
        planeCamera.gameObject.SetActive(true);
        planeController.enabled = true;
        isInside = true;
        TogglePlaneHUD(true);
        
        if (interactableController != null)
        {
            Debug.Log("Disabling InteractableController");
            interactableController.enabled = false;
        }
        else
        {
            Debug.LogWarning("interactableController is NULL!");
        }
        
        if (fuelBarCanvas != null)
        {
            fuelBarCanvas.SetActive(false);
        }
        if (progressText != null)
        {
            progressText.gameObject.SetActive(true);
            progressText.text = FindObjectOfType<CheckpointDetector>()?.progressText.text;
        }
    }

    void ExitPlane()
    {
        player.SetActive(true);
        planeCamera.gameObject.SetActive(false);
        planeController.enabled = false;
        isInside = false;
        TogglePlaneHUD(false);
        if (interactableController != null)
        {
            interactableController.enabled = true;
        }
        if (fuelBarCanvas != null)
        {
            fuelBarCanvas.SetActive(true);
        }
    }
}