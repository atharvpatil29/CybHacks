using UnityEngine;

public class ComputerInteraction2 : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    
    [Header("UI Control")]
    public GameObject passwordCheckUI;
    public FirstPersonMovement fpsMovement;

    private Renderer screenRenderer;
    private Rigidbody playerRigidbody;
    private bool isInteracting;

    void Start()
    {
        screenRenderer = GetComponent<Renderer>();
        screenRenderer.material = normalMaterial;
        passwordCheckUI.SetActive(false);
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
        fpsMovement.enabled = false;
        
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
        passwordCheckUI.SetActive(false);
        fpsMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}