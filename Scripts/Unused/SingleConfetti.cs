using UnityEngine;

public class SingleConfetti : MonoBehaviour
{
    private float duration = 1.2f;
    private float timeElapsed = 0f;
    private Vector2 direction;
    private float rotateSpeed;
    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float dist = Random.Range(40f, 100f) * 0.01f;
        direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * dist * 2f;
        rotateSpeed = Random.Range(180f, 360f);
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;

        transform.position += (Vector3)(direction * Time.deltaTime);
        transform.Rotate(Vector3.forward, rotateSpeed * Time.deltaTime);

        // fade out
        if (sr != null)
        {
            Color c = sr.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            sr.color = c;
        }
        if (t >= 1f) Destroy(gameObject);
    }
}
