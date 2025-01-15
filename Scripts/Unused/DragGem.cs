using UnityEngine;
using UnityEngine.UI;

public class DragGem : MonoBehaviour
{
    [SerializeField] private Image gemImage;

    public void Init(Sprite gemSprite, Vector2 position, float cellSize)
    {
        if (gemImage != null && gemSprite != null)
        {
            gemImage.sprite = gemSprite;
        }
        var rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = new Vector2(cellSize, cellSize);
        }
        transform.position = position;
    }
}
