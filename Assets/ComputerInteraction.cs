using UnityEngine;
using UnityEngine.EventSystems;

public class ComputerInteraction : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer screenRenderer;

    [Header("UI")]
    public GameObject passwordCheckUI;
    public FirstPersonMovement fpsMovement; // Reference to YOUR movement script
    private Rigidbody playerRigidbody;

    private bool isInteracting;

    void Start()
    {
        screenRenderer = GetComponent<Renderer>();
        screenRenderer.material = normalMaterial;
        passwordCheckUI.SetActive(false);
        
        // Get references
        playerRigidbody = fpsMovement.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screenRenderer.material = highlightMaterial;
            StartInteraction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            screenRenderer.material = normalMaterial;
            EndInteraction();
        }
    }

    void Update()
    {
        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        passwordCheckUI.SetActive(true);
        
        // Disable movement
        fpsMovement.enabled = false;
        
        // Stop physics movement
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        
        // Enable cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
        passwordCheckUI.SetActive(false);
        
        // Re-enable movement
        fpsMovement.enabled = true;
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}