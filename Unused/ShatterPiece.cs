using UnityEngine;

public class ShatterPiece : MonoBehaviour
{
    private float duration = 0.5f;
    private float timeElapsed = 0f;
    private Vector3 direction;
    private float speed;
    private SpriteRenderer sr;
    private Vector3 startScale;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        speed = 2f;
        startScale = transform.localScale;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;

        // move outward
        transform.position += direction * speed * Time.deltaTime;

        // shrink
        float sc = Mathf.Lerp(1f, 0.3f, t);
        transform.localScale = startScale * sc;

        // fade out
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }
        if (t >= 1f) Destroy(gameObject);
    }

    public void SetColor(Color color)
    {
        if (sr != null) sr.color = color;
    }
}
