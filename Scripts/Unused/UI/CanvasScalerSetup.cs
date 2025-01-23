using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// If you want automated UI scaling. Minimal logs: none unless a reference is missing.
/// </summary>
[ExecuteAlways]
public class CanvasScalerSetup : MonoBehaviour
{
    private void Start()
    {
        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler)
        {
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
        }
    }
}
