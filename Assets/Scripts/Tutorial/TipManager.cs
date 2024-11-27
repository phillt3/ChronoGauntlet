using UnityEngine;
using TMPro;
using System.Collections;
using System.Net.Sockets;

public class TipManager : MonoBehaviour
{
    public GameObject tipPanel;
    public CanvasGroup canvasGroup;   // Reference to the CanvasGroup for fading
    public GameObject tipTextObject;
    public bool TipWindowActive = false;
    private TextMeshProUGUI tipText;
    public static TipManager instance;

    private AudioSource audioSource;
    public AudioClip boxAppearSound;
    public AudioClip boxCloseSound;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        tipPanel.SetActive(false);
        tipText = tipTextObject.GetComponent<TextMeshProUGUI>();

        if (tipText == null) {
            Debug.LogError("Missing Tip Text Component from Panel!");
        }
        canvasGroup.alpha = 0;        // Start invisible

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.spatialBlend = 0f;
    }

    public void ShowTip(string message)
    {
        if (boxAppearSound != null) {
            audioSource.clip = boxAppearSound;
            audioSource.Play();
        }
            
        tipText.text = message;
        TipWindowActive = true;
        Cursor.visible = true;
        StartCoroutine(FadeIn());
    }

    public void CloseTip()
    {
        if (boxCloseSound != null) {
            audioSource.clip = boxCloseSound;
            audioSource.Play();
        }

        TipWindowActive = false;
        Cursor.visible = false;
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        tipPanel.SetActive(true);
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(0, 1, elapsedTime / duration);
            yield return null;
        }

        Time.timeScale = 0f;
    }

    private IEnumerator FadeOut()
    {
        //IF PANEL IS NOT CLOSING OUT CHECK THAT CLOSE BUTTON HAS PROPER ON CLICK ATTACHED
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.SmoothStep(1, 0, elapsedTime / duration);
            yield return null;
        }

        tipPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}
