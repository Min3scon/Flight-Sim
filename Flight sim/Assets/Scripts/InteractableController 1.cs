using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    public class InteractableController : MonoBehaviour
    {
        [SerializeField] Camera playerCamera;
        [SerializeField] TextMeshProUGUI interactionText;
        [SerializeField] float interactionDistance = 5f;

        IInteractable currentTargetedInteractable;

        void Update()
        {
            UpdateCurrentInteractable();
            UpdateInteractionText();
            CheckForInteractionInput();
        }

        void UpdateCurrentInteractable()
        {
            var ray = playerCamera.ViewportPointToRay(new Vector2(0.5f, 0.5f));

            if (Physics.Raycast(ray, out var hit, interactionDistance))
            {
                currentTargetedInteractable = hit.collider.GetComponent<IInteractable>() ??
                                              hit.collider.GetComponentInParent<IInteractable>();
            }
            else
            {
                currentTargetedInteractable = null;
            }
        }

        void UpdateInteractionText()
        {
            if (interactionText == null) return;

            if (currentTargetedInteractable != null)
            {
                interactionText.text = currentTargetedInteractable.InteractMessage;
            }
            else
            {
                interactionText.text = string.Empty;
            }
        }

        void CheckForInteractionInput()
        {
            if (Keyboard.current == null) return;

            if (Keyboard.current.eKey.wasPressedThisFrame && currentTargetedInteractable != null)
            {
                currentTargetedInteractable.Interact();
            }
        }
    }
}
