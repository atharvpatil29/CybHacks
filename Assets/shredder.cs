using UnityEngine;
using TMPro;

public class ShredderInteraction : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 1.5f;
    public KeyCode interactKey = KeyCode.E;
    public float shakeIntensity = 0.1f;
    public float shredDuration = 2f;
    public AudioClip shredSoundClip; // Changed to AudioClip

    [Header("References")]
    public FirstPersonMovement playerController;
    public TMP_Text interactPrompt;
    
    private Transform player;
    private Vector3 originalPosition;
    private bool isShredding;
    private bool inRange;
    private AudioSource audioSource; // Added AudioSource reference

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        originalPosition = transform.position;
        
        // Audio source setup from working script
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = shredSoundClip;
        audioSource.playOnAwake = false;
        
        if(interactPrompt) interactPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isShredding) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = inRange;
        inRange = distance <= interactRange;

        if(wasInRange != inRange)
        {
            if(interactPrompt) interactPrompt.gameObject.SetActive(inRange && !isShredding);
        }

        if (inRange && Input.GetKeyDown(interactKey))
        {
            StartCoroutine(ShredEffect());
        }

        AnimatePrompt();
    }

    System.Collections.IEnumerator ShredEffect()
    {
        isShredding = true;
        if(interactPrompt) interactPrompt.gameObject.SetActive(false);
        
        // Play sound using the audio source
        if (audioSource && shredSoundClip)
        {
            audioSource.Play();
        }
        
        Vector3 startPosition = transform.position;
float elapsed = 0f;

        while (elapsed < shredDuration)
        {
            transform.position = startPosition + Random.insideUnitSphere * shakeIntensity;
            shakeIntensity = Mathf.Lerp(0.1f, 0.3f, elapsed/shredDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition;
        isShredding = false;
        if(inRange && interactPrompt) interactPrompt.gameObject.SetActive(true);
    }

    void AnimatePrompt()
    {
        if(interactPrompt && interactPrompt.gameObject.activeSelf)
        {
            interactPrompt.color = Color.Lerp(
                Color.white, 
                Color.red, 
                Mathf.PingPong(Time.time * 2, 1)
            );
            
            interactPrompt.transform.localScale = Vector3.one * 
                (1 + Mathf.Sin(Time.time * 5) * 0.1f);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}