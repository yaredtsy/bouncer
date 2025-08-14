using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [Header("Fence Settings")]
    [SerializeField] private bool isResponsiveFence = true;
    [SerializeField] private float fenceThickness = 1f;
    [SerializeField] private float screenMargin = 0.1f;
    
    private Camera mainCamera;

    void Start()
    {
        // Only run responsive fence logic on parent
        if (isResponsiveFence && (transform.parent == null || transform.parent.name.ToLower().Contains("playground")))
        {
            InitializeFence();
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        IDestroyer destroyer = other.gameObject.GetComponent<IDestroyer>();
        if (destroyer != null)
        {
            destroyer.OnClean();
        }
    }

    private void InitializeFence()
    {
        mainCamera = Camera.main;
        if (mainCamera == null) return;
        
        SetupFenceBlocks();
    }

    private void SetupFenceBlocks()
    {
        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        
        // Calculate edges and sizes (reduced by half)
        float leftEdge = -screenBounds.x + screenMargin;
        float rightEdge = screenBounds.x - screenMargin;
        float topEdge = screenBounds.y - screenMargin;
        float bottomEdge = -screenBounds.y + screenMargin;
        
        float verticalSize = (topEdge - bottomEdge) * 0.5f;
        float horizontalSize = (rightEdge - leftEdge) * 0.5f;

        // Find and position fence blocks
        Destroyer[] allBlocks = GetComponentsInChildren<Destroyer>();
        List<Destroyer> fenceBlocks = new List<Destroyer>();
        
        foreach (Destroyer block in allBlocks)
        {
            if (block != this) fenceBlocks.Add(block);
        }

        if (fenceBlocks.Count == 4)
        {
            // Position 4 blocks at edges
            SetupBlock(fenceBlocks[0].transform, leftEdge, 0, fenceThickness, verticalSize);      // Left
            SetupBlock(fenceBlocks[1].transform, rightEdge, 0, fenceThickness, verticalSize);     // Right
            SetupBlock(fenceBlocks[2].transform, 0, topEdge, horizontalSize, fenceThickness);    // Top
            SetupBlock(fenceBlocks[3].transform, 0, bottomEdge, horizontalSize, fenceThickness); // Bottom
        }
    }

    private void SetupBlock(Transform block, float x, float y, float width, float height)
    {
        block.position = new Vector3(x, y, block.position.z);
        block.localScale = new Vector3(width, height, 1f);
    }
}
