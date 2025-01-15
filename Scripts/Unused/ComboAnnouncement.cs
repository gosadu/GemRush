using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ComboAnnouncement : MonoBehaviour
{
    [SerializeField] private Text comboText;
    [SerializeField] private Image comboGifImage;
    private float timeElapsed = 0f;
    private float showDuration = 1.2f;
    private bool isActive = false;

    public void Show(int multiplier)
    {
        isActive = true;
        if (multiplier == 99)
        {
            if (comboText != null) comboText.text = "EPIC SWAP!";
            // Possibly set comboGifImage sprite to a big combo
        }
        else
        {
            if (comboText != null) comboText.text = "CHAIN x" + multiplier + "!";
        }
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!isActive) return;
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= showDuration)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
