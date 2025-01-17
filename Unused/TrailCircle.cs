using UnityEngine;

public class TrailCircle : MonoBehaviour
{
    private float duration = 0.5f;
    private float timeElapsed = 0f;
    private Vector3 startScale;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;
        float alpha = Mathf.Lerp(1f, 0f, t);
        transform.localScale = Vector3.Lerp(startScale, startScale * 1.5f, t);
        if (sr != null)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        if (timeElapsed >= duration)
        {
            Destroy(gameObject);
        }
    }
}
