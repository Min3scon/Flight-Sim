using UnityEngine;

public class RandomTextureOffset : MonoBehaviour
{
    public Renderer targetRenderer;         // The renderer whose material will be randomized
    public Vector2 tilingMin = new Vector2(1, 1);
    public Vector2 tilingMax = new Vector2(3, 3);

    void Start()
    {
        if (targetRenderer == null)
            targetRenderer = GetComponent<Renderer>();

        // Randomize tiling size within range
        float randomX = Random.Range(tilingMin.x, tilingMax.x);
        float randomY = Random.Range(tilingMin.y, tilingMax.y);

        // Apply random tiling
        targetRenderer.material.mainTextureScale = new Vector2(randomX, randomY);

        // Randomize texture offset
        float offsetX = Random.Range(0f, 1f);
        float offsetY = Random.Range(0f, 1f);
        targetRenderer.material.mainTextureOffset = new Vector2(offsetX, offsetY);
    }
}
