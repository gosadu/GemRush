using UnityEngine;

public class SparkEffect : MonoBehaviour
{
    public Vector2 velocity;
    public float duration = 0.5f;
    private float timeElapsed = 0f;
    private SpriteRenderer sr;
    private Vector3 startScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;

        // Move outward
        transform.position += (Vector3)(velocity * Time.deltaTime);

        // Scale up
        float sc = Mathf.Lerp(0.5f, 2f, t);
        transform.localScale = startScale * sc;

        // Fade out
        float alpha = Mathf.Lerp(1f, 0f, t);
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }

        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }
}
