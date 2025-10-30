using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class USBInteractionFinal : MonoBehaviour
{
    [Header("UI References")]
    public GameObject interactPrompt;
    public GameObject choicePanel;
    public TMP_Text resultText;

    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";
    public float messageDuration = 2f;
    public bool pauseGameDuringInteraction = true;

    private bool inRange;
    private bool interacting;

    void Start()
    {
        InitializeSystem();
    }

    void InitializeSystem()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        interactPrompt.SetActive(false);
        choicePanel.SetActive(false);
        resultText.gameObject.SetActive(false);
        
        if (!FindObjectOfType<EventSystem>())
        {
            gameObject.AddComponent<EventSystem>();
            gameObject.AddComponent<StandaloneInputModule>();
        }
    }

    void Update()
    {
        HandleInteraction();
        HandleEscape();
    }

    void HandleInteraction()
    {
        if (inRange && !interacting && Input.GetKeyDown(interactKey))
        {
            StartInteraction();
        }
    }

    void HandleEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (choicePanel.activeSelf) CancelInteraction();
            else if (resultText.gameObject.activeSelf) ForceClose();
        }
    }

    void StartInteraction()
    {
        interacting = true;
        interactPrompt.SetActive(false);
        choicePanel.SetActive(true);
        ToggleCursor(false);
        if (pauseGameDuringInteraction) Time.timeScale = 0;
    }

    public void SelectPlugIn() => ShowOutcome("<color=red>RANSOMWARE ACTIVATED!</color>");
    public void SelectReport() => ShowOutcome("<color=green>USB REPORTED!</color>");

    void ShowOutcome(string message)
    {
        choicePanel.SetActive(false);
        resultText.text = message;
        resultText.gameObject.SetActive(true);
        StartCoroutine(ResetInteraction());
    }

    IEnumerator ResetInteraction()
    {
        yield return new WaitForSecondsRealtime(messageDuration);
        CleanupInteraction();
    }

    void CancelInteraction() => CleanupInteraction();

    void ForceClose()
    {
        StopAllCoroutines();
        CleanupInteraction();
    }

    void CleanupInteraction()
    {
        // Deactivate all UI elements
        resultText.gameObject.SetActive(false);
        choicePanel.SetActive(false); // Added line to fix the issue
        interacting = false;
        ToggleCursor(true);
        
        if (pauseGameDuringInteraction) 
            Time.timeScale = 1;

        if (inRange)
            interactPrompt.SetActive(true);
    }

    void ToggleCursor(bool hide)
    {
        Cursor.lockState = hide ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !hide;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = true;
            if (!interacting) interactPrompt.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            inRange = false;
            interactPrompt.SetActive(false);
        }
    }
}