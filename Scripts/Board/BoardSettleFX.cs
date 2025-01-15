using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// BoardSettleFX: subtle exhale glow across the board after final cascade.
/// </summary>
public class BoardSettleFX : MonoBehaviour
{
    public IEnumerator DoBoardExhale(GemView[] allGems, float pulseDuration)
    {
        float time = 0f;
        while (time < pulseDuration)
        {
            time += Time.deltaTime;
            float t = time / pulseDuration;
            float glow = Mathf.Sin(Mathf.PI * t);
            foreach (var gv in allGems)
            {
                Image img = gv.GetComponent<Image>();
                if (img)
                {
                    Color c = img.color;
                    float lighten = 0.1f * glow;
                    c.r = Mathf.Clamp01(c.r + lighten);
                    c.g = Mathf.Clamp01(c.g + lighten);
                    c.b = Mathf.Clamp01(c.b + lighten);
                    img.color = c;
                }
            }
            yield return null;
        }
        // Optionally reset color if needed
    }
}
