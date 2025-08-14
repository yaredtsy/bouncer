using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FitToCamera : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;

    void Awake()
    {
        // Get the components we need
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCamera = Camera.main;
    }

    void Start()
    {
        if (spriteRenderer.sprite == null)
        {
            Debug.LogError("FitToCamera: No sprite assigned to the SpriteRenderer.");
            return;
        }

        // Set the object's position to be the same as the camera's, but with its own Z-depth.
        // This centers the background on the camera.
        transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, transform.position.z);

        // Calculate the camera's height and width in world units
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        // Get the sprite's original size in world units
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;
        float spriteWidth = spriteRenderer.sprite.bounds.size.x;

        // Calculate the required scale to fit the screen
        float scaleX = cameraWidth / spriteWidth;
        float scaleY = cameraHeight / spriteHeight;

        // Apply the new scale to the GameObject's transform.
        // We use the same scale factor for both X and Y to maintain the aspect ratio.
        // Using Mathf.Max ensures the sprite covers the entire screen, even if aspect ratios differ.
        float finalScale = Mathf.Max(scaleX, scaleY);
        transform.localScale = new Vector3(finalScale, finalScale, 1f);
    }
}