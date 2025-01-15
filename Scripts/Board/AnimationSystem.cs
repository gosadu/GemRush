// FILE: AnimationSystem.cs
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Handles fancy animations or effects for gem removal, shockwaves, etc.
/// Calls EnhancedBoardManager.RemoveGem and RedrawBoard as needed.
/// </summary>
public class AnimationSystem : MonoBehaviour
{
    public float vanishDuration = 0.3f;

    public GameObject gemShatterPrefab; 
    public GameObject shockwavePrefab;

    public void AnimateGemRemoval(List<GemData> gemsToRemove, EnhancedBoardManager boardMgr)
    {
        StartCoroutine(DoFancyRemoval(gemsToRemove, boardMgr));
    }

    private IEnumerator DoFancyRemoval(List<GemData> gemsToRemove, EnhancedBoardManager boardMgr)
    {
        // gather gemViews
        List<GemView> gemViews = new List<GemView>();
        // Instead of FindObjectsOfType, we do:
        GemView[] allGems = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var gv in allGems)
        {
            if (gemsToRemove.Contains(gv.gemData))
            {
                gemViews.Add(gv);
            }
        }

        // vanish + shatter
        yield return StartCoroutine(ShatterAndFade(gemViews));

        // shockwave
        if (shockwavePrefab)
        {
            foreach (var gv in gemViews)
            {
                Instantiate(shockwavePrefab, gv.transform.position, Quaternion.identity);
            }
        }

        // remove from board
        foreach (var gv in gemViews)
        {
            boardMgr.RemoveGem(gv.gemData);
            Destroy(gv.gameObject);
        }

        // redraw
        boardMgr.RedrawBoard();
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
                Instantiate(gemShatterPrefab, gv.transform.position, Quaternion.identity);
            }
        }
        yield return null;
    }
}
