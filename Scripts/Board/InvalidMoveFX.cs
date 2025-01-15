using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// InvalidMoveFX: quick "nope" effect (shake + color shift) if a swap is invalid.
/// </summary>
public class InvalidMoveFX : MonoBehaviour
{
    public IEnumerator DoInvalidMove(RectTransform rt, float duration = 0.25f)
    {
        Vector2 originalPos = rt.anchoredPosition;
        float time = 0f;

        Image img = rt.GetComponent<Image>();
        Color originalColor = (img) ? img.color : Color.white;
        if (img)
        {
            img.color = Color.Lerp(originalColor, Color.gray, 0.5f);
        }

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float shakeMag = Mathf.Sin(t * 20f) * 10f;
            rt.anchoredPosition = originalPos + new Vector2(shakeMag, 0f);
            yield return null;
        }

        rt.anchoredPosition = originalPos;
        if (img) img.color = originalColor;
    }
}
