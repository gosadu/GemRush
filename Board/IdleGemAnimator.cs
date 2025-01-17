using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// IdleGemAnimator: subtle bobbing & twinkle when gem is idle.
/// </summary>
public class IdleGemAnimator : MonoBehaviour
{
    public float bobAmplitude = 5f;       // Vertical bob range in px
    public float bobSpeed = 2f;          // Speed of the bobbing
    public float twinkleIntervalMin = 3f;
    public float twinkleIntervalMax = 8f;

    private RectTransform rt;
    private Vector2 initPos;
    private float bobTimer = 0f;
    private bool isReady = false;

    void Awake()
    {
        // Grab our RectTransform, but don't set initPos yet.
        rt = GetComponent<RectTransform>();
    }

    IEnumerator Start()
    {
        // Wait until the end of the first rendered frame to ensure
        // EnhancedBoardManager (or other code) has positioned us.
        yield return new WaitForEndOfFrame();

        if (rt)
        {
            // Capture the final anchoredPosition as our idle "base."
            initPos = rt.anchoredPosition;
            isReady = true;

            // Start the random twinkle coroutine (optional idle sparkle).
            StartCoroutine(DoRandomTwinkles());
        }
    }

    void Update()
    {
        // If we're not ready or lost the RectTransform, do nothing.
        if (!isReady || !rt) return;

        // Increment a simple timer for the bob animation
        bobTimer += Time.deltaTime * bobSpeed;

        // Apply a vertical offset around initPos
        float offset = Mathf.Sin(bobTimer) * bobAmplitude;
        rt.anchoredPosition = new Vector2(initPos.x, initPos.y + offset);

        // Optional gentle rotation
        float rotZ = Mathf.Sin(bobTimer * 0.5f) * 5f;
        rt.localRotation = Quaternion.Euler(0, 0, rotZ);
    }

    private IEnumerator DoRandomTwinkles()
    {
        Image img = GetComponent<Image>();
        if (!img) yield break;

        while (true)
        {
            if (!img) yield break;

            // Wait a random interval between twinkles
            float wait = Random.Range(twinkleIntervalMin, twinkleIntervalMax);
            yield return new WaitForSeconds(wait);

            // A short "twinkle" animation modulating the alpha
            float twinkleDur = 0.2f;
            float t = 0f;
            while (t < twinkleDur)
            {
                if (!img) yield break;

                t += Time.deltaTime;
                float alpha = 1f + 0.5f * Mathf.Sin((t / twinkleDur) * Mathf.PI * 2f);
                Color c = img.color;
                c.a = alpha;
                img.color = c;
                yield return null;
            }

            // Restore full alpha
            if (img)
            {
                Color rc = img.color;
                rc.a = 1f;
                img.color = rc;
            }
        }
    }

    void OnDestroy()
    {
        // Stop all coroutines on destruction
        StopAllCoroutines();
    }
}
