using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Handles match clearing animations: build-up pulse, vanish/shatter, shockwave, etc.
/// </summary>
public class AnimationSystem : MonoBehaviour
{
    public float vanishDuration = 0.3f;

    public GameObject gemShatterPrefab;
    public GameObject shockwavePrefab;

    public void AnimateGemRemoval(List<GemData> gemsToRemove, GemData[,] board, EnhancedBoardManager boardMgr)
    {
        StartCoroutine(DoFancyRemoval(gemsToRemove, board, boardMgr));
    }

    private IEnumerator DoFancyRemoval(List<GemData> gemsToRemove, GemData[,] board, EnhancedBoardManager boardMgr)
    {
        // gather gemViews
        List<GemView> gemViews = new List<GemView>();
        GemView[] allGems = FindObjectsOfType<GemView>();
        foreach (var gv in allGems)
        {
            if (gv != null && gemsToRemove.Contains(gv.gemData))
            {
                gemViews.Add(gv);
            }
        }

        // build-up
        float buildUpDur = 0.25f;
        yield return StartCoroutine(BuildUpEffect(gemViews, buildUpDur));

        // vanish + shatter
        yield return StartCoroutine(ShatterAndFade(gemViews));

        // shockwave
        if (shockwavePrefab)
        {
            foreach (var gv in gemViews)
            {
                if (gv)
                {
                    Instantiate(shockwavePrefab, gv.transform.position, Quaternion.identity);
                }
            }
        }

        // remove from board
        foreach (var gv in gemViews)
        {
            if (gv)
            {
                boardMgr.RemoveGem(gv.gemData);
                Destroy(gv.gameObject);
            }
        }

        // redraw
        boardMgr.RedrawBoard();
    }

    private IEnumerator BuildUpEffect(List<GemView> gemViews, float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float scale = 1f + 0.2f * Mathf.Sin(t * Mathf.PI * 2f);
            foreach (var gv in gemViews)
            {
                if (gv) gv.transform.localScale = Vector3.one * scale;
            }
            yield return null;
        }
        foreach (var gv in gemViews)
        {
            if (gv) gv.transform.localScale = Vector3.one;
        }
    }

    private IEnumerator ShatterAndFade(List<GemView> gemViews)
    {
        float time = 0f;
        while (time < vanishDuration)
        {
            time += Time.deltaTime;
            float alpha = 1f - (time / vanishDuration);
            foreach (var gv in gemViews)
            {
                if (!gv) continue;

                Image img = gv.GetComponent<Image>();
                if (img)
                {
                    Color c = img.color;
                    c.a = alpha;
                    img.color = c;
                }
            }
            yield return null;
        }

        if (gemShatterPrefab)
        {
            foreach (var gv in gemViews)
            {
                if (gv)
                {
                    Instantiate(gemShatterPrefab, gv.transform.position, Quaternion.identity);
                }
            }
        }
        yield return null;
    }
}
