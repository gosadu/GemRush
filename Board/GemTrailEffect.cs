using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// GemTrailEffect: attaches color-based trailing particles to a gem as it moves.
/// Requires a ParticleSystem child (trailParticleSystem).
/// </summary>
public class GemTrailEffect : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem trailParticleSystem;

    private RectTransform rectT;
    private Image gemImage;

    void Awake()
    {
        rectT = GetComponent<RectTransform>();
        gemImage = GetComponent<Image>();
    }

    void Start()
    {
        // Color the particle system to match gem color (approx)
        if (trailParticleSystem && gemImage)
        {
            var main = trailParticleSystem.main;
            main.startColor = gemImage.color;
        }
    }

    void Update()
    {
        // Keep the trail at gem's position if you want real-time trailing
        if (trailParticleSystem)
        {
            trailParticleSystem.transform.position = rectT.position;
        }
    }
}
