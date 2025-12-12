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
    bool isInside;

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
    }

    void FindUITexts()
    {
        interactionText = GameObject.Find("Interaction")?.GetComponent<TextMeshProUGUI>();
        speedText = GameObject.Find("speed")?.GetComponent<TextMeshProUGUI>();
        altitudeText = GameObject.Find("altitude")?.GetComponent<TextMeshProUGUI>();
        fuelHud = GameObject.Find("FuelText")?.GetComponent<TextMeshProUGUI>();
        progressText = GameObject.Find("checkpoints")?.GetComponent<TextMeshProUGUI>();
    }

    void TogglePlaneHUD(bool show)
    {
        if (speedText != null) speedText.gameObject.SetActive(show);
        if (altitudeText != null) altitudeText.gameObject.SetActive(show);
        if (fuelHud != null) fuelHud.gameObject.SetActive(show);
        if (progressText != null) progressText.gameObject.SetActive(!show);
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
        if (!isInside) EnterPlane();
        else ExitPlane();
    }

    void EnterPlane()
    {
        if (interactionText != null) interactionText.text = "";

        player.SetActive(false);
        planeCamera.gameObject.SetActive(true);
        planeController.enabled = true;
        isInside = true;
        TogglePlaneHUD(true);

        if (interactableController != null) interactableController.enabled = false;
        if (fuelBarCanvas != null) fuelBarCanvas.SetActive(false);

        var detector = FindCheckpointDetector();
        if (progressText != null && detector != null)
        {
            progressText.gameObject.SetActive(true);
            if (detector.ProgressText != null)
                progressText.text = detector.ProgressText.text;
        }
    }

    void ExitPlane()
    {
        player.SetActive(true);
        planeCamera.gameObject.SetActive(false);
        planeController.enabled = false;
        isInside = false;
        TogglePlaneHUD(false);
        if (interactableController != null) interactableController.enabled = true;
        if (fuelBarCanvas != null) fuelBarCanvas.SetActive(true);
    }

    CheckpointDetector FindCheckpointDetector()
    {
#if UNITY_2023_1_OR_NEWER
        return Object.FindFirstObjectByType<CheckpointDetector>();
#else
        return Object.FindObjectOfType<CheckpointDetector>();
#endif
    }
}