using UnityEngine;

public class StickyNoteInteraction : MonoBehaviour
{
    [Header("UI References")]
    public GameObject stickyNotePanel;        // The actual note panel with text
    public GameObject interactionPrompt;      // The "Press E to Read" prompt

    [Header("Key Settings")]
    public KeyCode interactionKey = KeyCode.E;
    public string playerTag = "Player";

    private bool isPlayerInRange;

    void Start()
    {
        // Make sure both are hidden initially
        if (stickyNotePanel != null)
            stickyNotePanel.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    void Update()
    {
        // If the player is in range, pressing E toggles the note
        if (isPlayerInRange && Input.GetKeyDown(interactionKey))
        {
            ToggleStickyNotePanel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;

            // Show the "Press E" prompt if assigned
            if (interactionPrompt != null)
                interactionPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;

            // Hide everything upon exit
            if (stickyNotePanel != null)
                stickyNotePanel.SetActive(false);

            if (interactionPrompt != null)
                interactionPrompt.SetActive(false);
        }
    }

    private void ToggleStickyNotePanel()
    {
        if (stickyNotePanel == null) return;

        bool isActive = stickyNotePanel.activeSelf;
        stickyNotePanel.SetActive(!isActive);

        // Optionally hide the prompt once we've opened it
        if (isActive == false && interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
}