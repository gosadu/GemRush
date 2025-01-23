using UnityEngine;
using UnityEngine.UI;

public class PauseOverlay : MonoBehaviour
{
    [SerializeField] private Button resumeButton;

    void Awake()
    {
        if (resumeButton) resumeButton.onClick.AddListener(OnResumeClicked);
    }

    void OnResumeClicked()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowPauseOverlay()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
}
