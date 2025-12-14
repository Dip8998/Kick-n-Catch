using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraAspectFitter : MonoBehaviour
{
    [Header("Target Aspect (9:16)")]
    public float targetAspect = 9f / 16f;

    [Header("Design Orthographic Size")]
    public float designOrthoSize = 5f;

    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
        Apply();
    }

    void Apply()
    {
        float screenAspect = (float)Screen.width / Screen.height;

        if (screenAspect >= targetAspect)
        {
            cam.orthographicSize = designOrthoSize;
        }
        else
        {
            float scale = screenAspect / targetAspect;
            cam.orthographicSize = designOrthoSize / scale;
        }
    }
}
