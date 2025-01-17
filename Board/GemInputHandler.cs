using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// GemInputHandler: IPointerDown/Up to detect drag direction for swaps, with a smooth scale effect during drag.
/// </summary>
[RequireComponent(typeof(GemView))]
public class GemInputHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private GemView gemView;
    private Vector2 startPos;
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
        // optional highlight or partial follow
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

            if (Mathf.Abs(angle) < 45f) deltaCol = 1;
            else if (angle > 45f && angle < 135f) deltaRow = -1;
            else if (Mathf.Abs(angle) > 135f) deltaCol = -1;
            else if (angle < -45f && angle > -135f) deltaRow = 1;

            EnhancedBoardManager bm = FindObjectOfType<EnhancedBoardManager>();
            if (bm != null)
            {
                int newR = gemView.gemData.row + deltaRow;
                int newC = gemView.gemData.col + deltaCol;
                GemView other = FindGemView(newR, newC);
                if (other) gemView.SwapWith(other);
            }
        }
    }

    private GemView FindGemView(int r, int c)
    {
        GemView[] all = FindObjectsOfType<GemView>();
        foreach (var gv in all)
        {
            if (gv.gemData.row == r && gv.gemData.col == c) return gv;
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
