using UnityEngine;

/// <summary>
/// Re-checks the device's safe area each frame. If changed, 
/// anchors are computed and CLAMPED to [0..1], so it can never stretch beyond the screen.
/// This prevents the panel from expanding in weird scenarios or large Screen.safeArea values.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class ClampedCheckSafeAreaFitter : MonoBehaviour
{
    private RectTransform panel;
    private Rect lastSafeArea;

    void Awake()
    {
        panel = GetComponent<RectTransform>();
        lastSafeArea= new Rect(0,0,0,0);
        Refresh();
    }

    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        Rect area = Screen.safeArea;
        if(area == lastSafeArea) return; // skip if no change
        lastSafeArea = area;

        // Convert safeArea -> anchorMin/Max
        Vector2 anchorMin = new Vector2(area.xMin / Screen.width, area.yMin / Screen.height);
        Vector2 anchorMax = new Vector2((area.xMin + area.width)/Screen.width, (area.yMin + area.height)/Screen.height);

        // IMPORTANT: clamp to 0..1 so we never exceed the canvas range
        anchorMin.x = Mathf.Clamp01(anchorMin.x);
        anchorMin.y = Mathf.Clamp01(anchorMin.y);
        anchorMax.x = Mathf.Clamp01(anchorMax.x);
        anchorMax.y = Mathf.Clamp01(anchorMax.y);

        // In case the safe area (somehow) flips, ensure anchorMax≥anchorMin
        anchorMin.x = Mathf.Min(anchorMin.x, anchorMax.x);
        anchorMin.y = Mathf.Min(anchorMin.y, anchorMax.y);

        // Apply
        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;

        // Debug (optional): check final anchors
        Debug.Log($"[SafeArea] new anchorMin={anchorMin} anchorMax={anchorMax}");
    }
}
