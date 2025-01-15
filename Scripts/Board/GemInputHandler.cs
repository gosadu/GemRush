// FILE: GemInputHandler.cs
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Handles user input (pointer down/up/drag) on a gem for swapping in a match-3 game.
/// Updated to remove obsolete Object.FindObjectOfType calls & avoid unused variable warnings.
/// </summary>
[RequireComponent(typeof(GemView))]
public class GemInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GemView gemView;
    private Vector2 startPos;
    
    // We now actually use 'dragging' in OnDrag to remove the CS0414 warning.
    private bool dragging = false; 

    [Header("Drag Animation Settings")]
    public float dragScaleFactor = 1.2f;
    public float scaleLerpSpeed = 10f;

    void Awake()
    {
        gemView = GetComponent<GemView>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        dragging = true;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(dragScaleFactor));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragging)
        {
            // You can add optional drag visuals or gem follow here if you like.
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
        StopAllCoroutines();
        StartCoroutine(ScaleTo(1f));

        Vector2 endPos = eventData.position;
        Vector2 delta = endPos - startPos;
        if (delta.magnitude > 40f)
        {
            float angle = Vector2.SignedAngle(Vector2.right, delta);
            int deltaRow = 0;
            int deltaCol = 0;

            // Determine direction
            if (Mathf.Abs(angle) < 45f) deltaCol = 1;
            else if (angle > 45f && angle < 135f) deltaRow = -1;
            else if (Mathf.Abs(angle) > 135f) deltaCol = -1;
            else if (angle < -45f && angle > -135f) deltaRow = 1;

            // Instead of using FindObjectOfType, we do:
            EnhancedBoardManager bm = Object.FindAnyObjectByType<EnhancedBoardManager>();
            if (bm != null)
            {
                int newR = gemView.gemData.row + deltaRow;
                int newC = gemView.gemData.col + deltaCol;

                // We find the relevant gem via a direct approach 
                // or a quick search among GemViews:
                GemView other = FindGemView(newR, newC);
                if (other) gemView.SwapWith(other);
            }
        }
    }

    private GemView FindGemView(int r, int c)
    {
        // No more obsolete calls:
        GemView[] all = Object.FindObjectsByType<GemView>(
            FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (var gv in all)
        {
            if (gv.gemData != null &&
                gv.gemData.row == r && gv.gemData.col == c)
                return gv;
        }
        return null;
    }

    private IEnumerator ScaleTo(float targetScale)
    {
        while (true)
        {
            float cur = transform.localScale.x;
            float newS = Mathf.Lerp(cur, targetScale, Time.deltaTime * scaleLerpSpeed);
            transform.localScale = new Vector3(newS, newS, newS);
            if (Mathf.Abs(newS - targetScale) < 0.01f)
            {
                transform.localScale = Vector3.one * targetScale;
                yield break;
            }
            yield return null;
        }
    }
}
