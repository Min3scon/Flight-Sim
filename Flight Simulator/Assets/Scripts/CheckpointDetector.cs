using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CheckpointDetector : MonoBehaviour
{
    [Header("Checkpoints")]
    [SerializeField] Collider[] checkpoints;
    [SerializeField] TextMeshProUGUI progressText;

    [Header("Audio Clips")]
    [SerializeField] AudioClip checkpointClip;
    [SerializeField] AudioClip completedClip;

    AudioSource audioSource;
    int currentCheckpoint;
    bool wasInside;

    public TextMeshProUGUI ProgressText => progressText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        UpdateCheckpointVisibility();
        UpdateText();

        if (checkpoints != null && checkpoints.Length > 0)
            wasInside = checkpoints[0].bounds.Contains(transform.position);
    }

    void Update()
    {
        if (currentCheckpoint >= checkpoints.Length) return;

        Collider nextCheckpoint = checkpoints[currentCheckpoint];
        bool inside = nextCheckpoint.bounds.Contains(transform.position);

        if (inside && !wasInside)
        {
            currentCheckpoint++;
            PlayCheckpointSound();
            UpdateCheckpointVisibility();
            UpdateText();

            if (currentCheckpoint >= checkpoints.Length)
            {
                PlayCompletedSound();
                Debug.Log("ALL CHECKPOINTS COMPLETE!");
                wasInside = false;
                return;
            }
        }

        wasInside = inside;
    }

    void PlayCheckpointSound()
    {
        if (audioSource != null && checkpointClip != null)
            audioSource.PlayOneShot(checkpointClip);
    }

    void PlayCompletedSound()
    {
        if (audioSource != null && completedClip != null)
            audioSource.PlayOneShot(completedClip);
    }

    void UpdateCheckpointVisibility()
    {
        for (int i = 0; i < checkpoints.Length; i++)
            checkpoints[i].gameObject.SetActive(i == currentCheckpoint);
        wasInside = false;
    }

    void UpdateText()
    {
        if (progressText != null)
            progressText.text = currentCheckpoint + "/" + checkpoints.Length;
    }
}