using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedGIFOverlay : MonoBehaviour
{
    [SerializeField] private Image gifImage; // The Image for the "gif"
    private CanvasGroup canvasGroup;
    private float duration = 0.8f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show(Sprite gifSprite, float duration = 0.8f)
    {
        this.duration = duration;
        if (gifImage != null && gifSprite != null)
        {
            gifImage.sprite = gifSprite;
        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            gameObject.SetActive(true);
            StartCoroutine(DoFadeOut());
        }
    }

    private IEnumerator DoFadeOut()
    {
        yield return new WaitForSeconds(duration);
        float t = 0f;
        float fadeTime = 0.4f;
        while (t < fadeTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / fadeTime);
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvasGroup.alpha = 0f;
        gameObject.SetActive(false);
    }
}
