using UnityEngine;
using UnityEngine.Tilemaps;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Tilemap targetTilemap;
    public float transparentAlpha = 0.5f;
    public float fadeSpeed = 2f;

    private float currentTargetAlpha;
    private bool isPlayerBehind = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTargetAlpha = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetTilemap == null) return;
        Color currentColor = targetTilemap.color;
        float newAlpha = Mathf.Lerp(currentColor.a, currentTargetAlpha, Time.deltaTime * fadeSpeed);
        currentColor.a = newAlpha;
        targetTilemap.color = currentColor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerBehind = true;
            currentTargetAlpha = transparentAlpha;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerBehind = false;
            currentTargetAlpha = 1f;
        }
    }
}
