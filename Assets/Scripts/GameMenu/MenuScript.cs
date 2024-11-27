using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    private CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        EventSystem.current.SetSelectedGameObject(gameObject);
    }
    public void Resume()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0f;
        if (!TipManager.instance.TipWindowActive) {
            Time.timeScale = 1f;
            Cursor.visible = false;
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("LevelTutorial");
        Time.timeScale = 1f;
    }

    public void ShowCredits()
    {
        SceneManager.LoadScene("CreditScreen");
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("MenuScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (canvasGroup.interactable)
            {
                canvasGroup.interactable = false;
                canvasGroup.blocksRaycasts = false;
                canvasGroup.alpha = 0f;
                if (!TipManager.instance.TipWindowActive) {
                    Time.timeScale = 1f;
                    Cursor.visible = false;
                }
            }
            else
            {
                canvasGroup.interactable = true;
                canvasGroup.blocksRaycasts = true;
                canvasGroup.alpha = 1f;
                Time.timeScale = 0f;
                Cursor.visible = true;
            }
        }
    }
}
