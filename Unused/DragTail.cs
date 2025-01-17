using UnityEngine;
using System.Collections;

public class DragTrail : MonoBehaviour
{
    [SerializeField] private GameObject trailCirclePrefab;
    private float lastAddTime = 0f;
    private bool isActive = false;

    public void SetActive(bool active)
    {
        isActive = active;
        lastAddTime = Time.time;
    }

    public void UpdateTrail(Vector2 position)
    {
        if (!isActive) return;
        if (Time.time - lastAddTime > 0.08f)
        {
            AddCircle(position);
            lastAddTime = Time.time;
        }
    }

    private void AddCircle(Vector2 position)
    {
        if (!trailCirclePrefab) return;
        var circle = Instantiate(trailCirclePrefab, this.transform);
        circle.transform.position = position;
    }
}
