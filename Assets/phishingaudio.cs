using UnityEngine;
using TMPro;

public class SimpleAudioPlayer : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioClip callAudio;
    
    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.E;
    public float interactionDistance = 3f;
    public string playerTag = "Player";
    
    [Header("UI References")]
    public GameObject interactPrompt;
    
    private AudioSource audioSource;
    private Transform playerTransform;
    private bool isPlayerInRange = false;
    
    private void Start()
    {
        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = callAudio;
        audioSource.playOnAwake = false;
        
        // Find player
        playerTransform = GameObject.FindGameObjectWithTag(playerTag).transform;
        
        // Initialize UI
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }
    }
    
    private void Update()
    {
        // Check if player is in range and handle interactions
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            bool wasInRange = isPlayerInRange;
            isPlayerInRange = distanceToPlayer <= interactionDistance;
            
            // Update prompt visibility if range status changed
            if (wasInRange != isPlayerInRange && interactPrompt != null)
            {
                interactPrompt.SetActive(isPlayerInRange);
            }
            
            // Play audio when E is pressed
            if (isPlayerInRange && Input.GetKeyDown(interactionKey) && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
    
    // Handle trigger-based detection as an alternative to distance-based
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = true;
            if (interactPrompt != null)
            {
                interactPrompt.SetActive(true);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            isPlayerInRange = false;
            if (interactPrompt != null)
            {
                interactPrompt.SetActive(false);
            }
        }
    }
    
    // Visualize interaction range in editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionDistance);
    }
}