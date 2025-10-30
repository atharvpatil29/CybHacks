using UnityEngine;

public class PhoneInteraction : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer phoneRenderer;

    [Header("UI")]
    public GameObject phoneScamUI;
    public FirstPersonMovement fpsMovement;
    public PhoneScamManager scamManager;
    private Rigidbody playerRigidbody;

    private bool isInteracting;

    void Start()
    {
        phoneRenderer = GetComponent<Renderer>();
        phoneRenderer.material = normalMaterial;
        phoneScamUI.SetActive(false);
        playerRigidbody = fpsMovement.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isInteracting)
        {
            phoneRenderer.material = highlightMaterial;
            StartInteraction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            phoneRenderer.material = normalMaterial;
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
        phoneScamUI.SetActive(true);
        scamManager.ResetScenarios();
        
        fpsMovement.enabled = false;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
        phoneScamUI.SetActive(false);
        
        fpsMovement.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}