using UnityEngine;
using System.Collections;
public class StrengthTerminalInteraction : MonoBehaviour
{
    [Header("Visuals")]
    public Material normalMaterial;
    public Material highlightMaterial;
    private Renderer terminalRenderer;

    [Header("UI")]
    public GameObject strengthCheckUI;
    public FirstPersonMovement fpsMovement;
    private Rigidbody playerRigidbody;

    private bool isInteracting;

    void Start()
    {
        terminalRenderer = GetComponent<Renderer>();
        terminalRenderer.material = normalMaterial;
        strengthCheckUI.SetActive(false);
        playerRigidbody = fpsMovement.GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            terminalRenderer.material = highlightMaterial;
            StartInteraction();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            terminalRenderer.material = normalMaterial;
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
        strengthCheckUI.SetActive(true);
        
        fpsMovement.enabled = false;
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EndInteraction()
    {
        isInteracting = false;
        strengthCheckUI.SetActive(false);
        
        fpsMovement.enabled = true;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}