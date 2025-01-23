using UnityEngine;
using System.Collections;

/// <summary>
/// ArcSwapEffect: Tweens two RectTransforms along an arc for a smooth swap.
/// Includes subtle scale mid-flight and micro-bounce upon landing.
/// </summary>
public class ArcSwapEffect : MonoBehaviour
{
    public IEnumerator DoArcSwap(RectTransform r1, RectTransform r2, float duration, System.Action onComplete)
    {
        // Early null-check (if one gem was destroyed mid-swap).
        if (r1 == null || r2 == null) yield break;

        Vector2 startPos1 = r1.anchoredPosition;
        Vector2 startPos2 = r2.anchoredPosition;

        float time = 0f;
        float arcHeight = Vector2.Distance(startPos1, startPos2) * 0.4f;

        while (time < duration)
        {
            if (r1 == null || r2 == null) yield break;

            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);

            float easedT = EaseInOutCubic(t);

            // Lerp base positions
            Vector2 newPos1 = Vector2.Lerp(startPos1, startPos2, easedT);
            Vector2 newPos2 = Vector2.Lerp(startPos2, startPos1, easedT);

            // Arc offset
            float offset = Mathf.Sin(Mathf.PI * easedT) * arcHeight;
            newPos1.y += offset;
            newPos2.y += offset;

            r1.anchoredPosition = newPos1;
            r2.anchoredPosition = newPos2;

            // subtle scale
            float scale = 1f + 0.1f * Mathf.Sin(Mathf.PI * easedT);
            r1.localScale = Vector3.one * scale;
            r2.localScale = Vector3.one * scale;

            yield return null;
        }

        // micro-bounce
        if (r1 == null || r2 == null) yield break;
        yield return StartCoroutine(DoMicroBounce(r1, r2, 0.05f));

        if (r1) r1.localScale = Vector3.one;
        if (r2) r2.localScale = Vector3.one;

        if (onComplete != null) onComplete();
    }

    private IEnumerator DoMicroBounce(RectTransform r1, RectTransform r2, float bounceTime)
    {
        float t = 0f;
        while (t < bounceTime)
        {
            if (r1 == null || r2 == null) yield break;

            t += Time.deltaTime;
            float ratio = t / bounceTime;
            float scale = 1f + 0.05f * Mathf.Sin(ratio * Mathf.PI * 2f);
            r1.localScale = Vector3.one * scale;
            r2.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    private float EaseInOutCubic(float x)
    {
        if (x < 0.5f)
        {
            return 4f * x * x * x;
        }
        else
        {
            return 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;
        }
    }
}
