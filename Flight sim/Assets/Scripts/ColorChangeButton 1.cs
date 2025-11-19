using UnityEngine;

namespace DefaultNamespace
{
    public class ColorChangeButton : MonoBehaviour, IInteractable
    {
        [SerializeField] private Renderer targetCubeRenderer;
        
        public string InteractMessage => "Press E to change cube color";
        
        public void Interact()
        {
            if (targetCubeRenderer != null)
            {
                Color randomColor = new Color(
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f),
                    Random.Range(0f, 1f)
                );
                
                targetCubeRenderer.material.color = randomColor;
                Debug.Log($"Cube color changed to: {randomColor}");
            }
            else
            {
                Debug.LogWarning("Target cube renderer not assigned!");
            }
        }
    }
}