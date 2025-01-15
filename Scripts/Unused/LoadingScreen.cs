using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Image loadingImage;        
    [SerializeField] private Text loadingText;          
    [SerializeField] private Text loadingStatusText;    

    private bool showGif = true;

    public void Show(string status, bool showGif)
    {
        this.showGif = showGif;
        if (loadingPanel != null) loadingPanel.SetActive(true);
        if (loadingStatusText != null) loadingStatusText.text = status;
    }

    public void UpdateStatus(string status)
    {
        if (loadingStatusText != null) loadingStatusText.text = status;
    }

    public void Hide()
    {
        if (loadingPanel != null) loadingPanel.SetActive(false);
    }
}
