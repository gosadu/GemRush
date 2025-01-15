using UnityEngine;
using UnityEngine.UI;

public class FloatingScore : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private float duration = 0.6f;
    private float timeElapsed = 0f;
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 startScale;

    public void Init(int points, Vector3 position)
    {
        if (scoreText != null) scoreText.text = "+" + points;
        startPos = position;
        endPos = position + new Vector3(0, 50f, 0);
        startScale = transform.localScale;
        transform.position = startPos;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed / duration;
        transform.position = Vector3.Lerp(startPos, endPos, t);

        float scale = Mathf.Lerp(0.7f, 1f, t);
        transform.localScale = startScale * scale;

        if (scoreText != null)
        {
            Color c = scoreText.color;
            c.a = Mathf.Lerp(1f, 0f, t);
            scoreText.color = c;
        }
        if (t >= 1f) Destroy(gameObject);
    }
}
