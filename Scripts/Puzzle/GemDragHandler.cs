using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class GemDragHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public float dragThreshold = 50f;

    private Vector2 startPosScreen;
    private bool isDragging = false;
    private Gem gemRef;
    private PuzzleBoardManager boardManager;
    private Vector2Int gemBoardPos;

    void Awake()
    {
        gemRef = GetComponent<Gem>();
        // Replace the deprecated call:
        boardManager = Object.FindFirstObjectByType<PuzzleBoardManager>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(boardManager == null) return;
        transform.DOPunchScale(Vector3.one * 0.1f, 0.2f, 10, 1);
        startPosScreen = eventData.position;
        gemBoardPos = boardManager.GetBoardPos(gemRef);
        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(!isDragging) return;
        Vector2 delta = eventData.position - startPosScreen;
        if(delta.magnitude >= dragThreshold)
        {
            float absX = Mathf.Abs(delta.x);
            float absY = Mathf.Abs(delta.y);
            Vector2Int offset = Vector2Int.zero;

            if(absX > absY)
            {
                offset = (delta.x > 0)? new Vector2Int(1,0) : new Vector2Int(-1,0);
            }
            else
            {
                offset = (delta.y > 0)? new Vector2Int(0,1) : new Vector2Int(0,-1);
            }

            Vector2Int neighbor = gemBoardPos + offset;
            boardManager.TrySwap(gemBoardPos, neighbor);
            isDragging = false;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
    }
}
