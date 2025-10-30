// USBInteraction.cs
using UnityEngine;
using UnityEngine.EventSystems;

public class USBInteraction : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer usbRenderer;

    [Header("Interaction Settings")]
    public float interactionDistance = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public string playerTag = "Player";

    [Header("UI")]
    public GameObject usbScenarioUI;
    public GameObject interactPrompt;
    public FirstPersonMovement fpsMovement;
    
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private bool isPlayerInRange = false;
    private bool isInteracting = false;

    void Start()
    {
        usbRenderer = GetComponent<Renderer>();
        usbRenderer.material = normalMaterial;
        
        usbScenarioUI.SetActive(false);
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        playerRigidbody = fpsMovement.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            bool wasInRange = isPlayerInRange;
            isPlayerInRange = distanceToPlayer <= interactionDistance;
            
            if (wasInRange != isPlayerInRange)
            {
                if (interactPrompt != null)
                {
                    interactPrompt.SetActive(isPlayerInRange && !isInteracting);
                }
                
                usbRenderer.material = isPlayerInRange ? highlightMaterial : normalMaterial;
            }
            
            if (isPlayerInRange && Input.GetKeyDown(interactKey) && !isInteracting)
            {
                StartInteraction();
            }
        }
        
        if (isInteracting && Input.GetKeyDown(KeyCode.Escape))
        {
            EndInteraction();
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        usbScenarioUI.SetActive(true);
        
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
        
        fpsMovement.enabled = false;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
        usbScenarioUI.SetActive(false);
        
        if (interactPrompt != null && isPlayerInRange)
        {
            interactPrompt.SetActive(true);
        }
        
        fpsMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}