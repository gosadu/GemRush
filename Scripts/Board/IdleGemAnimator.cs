using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// IdleGemAnimator: subtle bobbing & twinkle when gem is idle.
/// </summary>
public class IdleGemAnimator : MonoBehaviour
{
    public float bobAmplitude = 5f;
    public float bobSpeed = 2f;
    public float twinkleIntervalMin = 3f;
    public float twinkleIntervalMax = 8f;

    private RectTransform rt;
    private Vector2 initPos;
    private float bobTimer = 0f;

    // ----------------------------------------------------------------------------
    // CHANGES:
    //  1) We moved "rt = GetComponent<RectTransform>(); initPos = rt.anchoredPosition;"
    //     into Start() instead of Awake().
    //  2) We now start the DoRandomTwinkles() in Start(), after position is set.
    // ----------------------------------------------------------------------------

    void Start()
    {
        rt = GetComponent<RectTransform>();
        // capture correct anchoredPosition now that the gem has been placed
        initPos = rt.anchoredPosition;

        // begin random twinkle after we know our real position
        StartCoroutine(DoRandomTwinkles());
    }

    void Update()
    {
        bobTimer += Time.deltaTime * bobSpeed;
        float offset = Mathf.Sin(bobTimer) * bobAmplitude;
        rt.anchoredPosition = new Vector2(initPos.x, initPos.y + offset);

        float rot = Mathf.Sin(bobTimer * 0.5f) * 5f;
        rt.localRotation = Quaternion.Euler(0, 0, rot);
    }

    private IEnumerator DoRandomTwinkles()
    {
        Image img = GetComponent<Image>();
        if (!img) yield break;

        while (true)
        {
            float wait = Random.Range(twinkleIntervalMin, twinkleIntervalMax);
            yield return new WaitForSeconds(wait);

            float twinkleDur = 0.2f;
            float t = 0f;
            while (t < twinkleDur)
            {
                t += Time.deltaTime;
                float alpha = 1f + 0.5f * Mathf.Sin((t / twinkleDur) * Mathf.PI * 2f);
                Color c = img.color;
                c.a = alpha;
                img.color = c;
                yield return null;
            }
            // restore alpha
            Color rc = img.color;
            rc.a = 1f;
            img.color = rc;
        }
    }
}
