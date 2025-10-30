using UnityEngine;
using UnityEngine.EventSystems;

public class PhishingQuizInteraction : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer quizBoardRenderer;

    [Header("Interaction Settings")]
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";

    [Header("UI")]
    public GameObject phishingQuizUI;
    public GameObject interactPrompt;
    public FirstPersonMovement fpsMovement; // Reference to YOUR movement script
    
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private bool isPlayerInRange = false;
    private bool isInteracting = false;

    void Start()
    {
        // Set up renderer
        quizBoardRenderer = GetComponent<Renderer>();
        quizBoardRenderer.material = normalMaterial;
        
        // Set up UI
        phishingQuizUI.SetActive(false);
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        
        // Find player
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        playerRigidbody = fpsMovement.GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Distance-based detection
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            bool wasInRange = isPlayerInRange;
            isPlayerInRange = distanceToPlayer <= interactionDistance;
            
            // Handle visual changes when range status changes
            if (wasInRange != isPlayerInRange)
            {
                // Update prompt visibility
                if (interactPrompt != null)
                {
                    interactPrompt.SetActive(isPlayerInRange && !isInteracting);
                }
                
                // Update material
                quizBoardRenderer.material = isPlayerInRange ? highlightMaterial : normalMaterial;
            }
            
            // Start interaction when E is pressed
            if (isPlayerInRange && Input.GetKeyDown(interactKey) && !isInteracting)
            {
                StartInteraction();
            }
        }
        
        // Handle escape key during interaction
        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        phishingQuizUI.SetActive(true);
        
        // Hide the prompt while interacting
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        
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
        isInteracting = false;  // CRITICAL: Reset interaction state
        phishingQuizUI.SetActive(false);
        
        // Show prompt again if still in range
        if (interactPrompt != null && isPlayerInRange)
        {
            interactPrompt.SetActive(true);
        }
        
        // Re-enable movement
        fpsMovement.enabled = true;
        
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    // Visualize interaction range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}