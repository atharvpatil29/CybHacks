using UnityEngine;

[ExecuteInEditMode]
public class Zoom : MonoBehaviour
{
    public new Camera camera;
    public float defaultFOV = 60;
    public float maxZoomFOV = 15;
    [Range(0, 1)] public float currentZoom;
    public float sensitivity = 1;

    void Awake()
    {
        // Safely get camera component
        camera = GetComponent<Camera>();
        if (camera != null)
        {
            defaultFOV = camera.fieldOfView;
        }
        else
        {
            Debug.LogError("No Camera component found on this GameObject!", this);
        }
    }

    void Update()
    {
        if (camera == null) return;

        // Handle zoom input
        float scrollDelta = Input.mouseScrollDelta.y;
        currentZoom += scrollDelta * sensitivity * 0.05f;
        currentZoom = Mathf.Clamp01(currentZoom);
        
        // Smoothly adjust FOV
        camera.fieldOfView = Mathf.Lerp(defaultFOV, maxZoomFOV, currentZoom);
    }
}