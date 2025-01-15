using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// GemView: The visual for a gem. Holds GemData, sets the sprite at runtime.
/// </summary>
public class GemView : MonoBehaviour
{
    [HideInInspector] public GemData gemData;
    private EnhancedBoardManager boardManager;
    public Image gemImage;

    public void InitGem(GemData data, Sprite sprite, EnhancedBoardManager mgr)
    {
        gemData = data;
        boardManager = mgr;
        if (gemImage && sprite)
        {
            gemImage.sprite = sprite;
        }
        StartCoroutine(SpawnAnimation());
    }

    private IEnumerator SpawnAnimation()
    {
        float duration = 0.2f;
        Vector3 initScale = Vector3.zero;
        Vector3 finalScale = Vector3.one;
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            transform.localScale = Vector3.Lerp(initScale, finalScale, t);
            yield return null;
        }
        transform.localScale = finalScale;
    }

    public void SwapWith(GemView other)
    {
        if (boardManager)
        {
            boardManager.SwapGems(this.gemData, other.gemData);
        }
    }
}
