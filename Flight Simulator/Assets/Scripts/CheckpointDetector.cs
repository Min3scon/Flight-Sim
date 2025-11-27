using UnityEngine;
using TMPro;

public class CheckpointDetector : MonoBehaviour
{
    [SerializeField] Collider[] checkpoints;
    public TextMeshProUGUI progressText;
    private int currentCheckpoint = 0;

    private void Start()
    {
        UpdateText();
    }

    private void Update()
    {
        if (currentCheckpoint >= checkpoints.Length) return;
        Collider nextCheckpoint = checkpoints[currentCheckpoint];
        if (nextCheckpoint.bounds.Contains(transform.position))
        {
            currentCheckpoint++;
            UpdateText();
            if (currentCheckpoint >= checkpoints.Length)
            {
                Debug.Log("ALL CHECKPOINTS COMPLETE!");
            }
        }
    }

    private void UpdateText()
    {
        if (progressText != null)
        {
            progressText.text = currentCheckpoint + "/" + checkpoints.Length;
        }
    }
}
