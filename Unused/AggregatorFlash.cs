using UnityEngine;
using System.Collections;

public class AggregatorFlash : MonoBehaviour
{
    public float duration = 0.8f;
    private float timeElapsed;

    // If you’re using an Image, you’d store a reference here to fade or scale:
    // public Image flashImage;

    void Start()
    {
        // e.g. quick scale up or set initial color
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        // If you want to scale or fade over time, do it here:
        // float t = timeElapsed / duration;
        // transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one * 2f, t);

        if (timeElapsed >= duration)
        {
            Destroy(gameObject);
        }
    }
}
