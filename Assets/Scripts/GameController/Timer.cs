using UnityEngine;

public class Timer : MonoBehaviour
{
    public float totalTime = 300f;
    private float timeRemaining;

    void Start()
    {
        timeRemaining = totalTime;
    }

    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            QuitGame();
        }
    }

    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        string timeText = $"{minutes:00}:{seconds:00}";

        // Create the GUIStyle for the timer
        GUIStyle timerStyle = new GUIStyle
        {
            fontSize = 30,
            fontStyle = FontStyle.Bold,
            alignment = TextAnchor.UpperRight
        };

        // Change the text color to red if less than 60 seconds remain
        if (timeRemaining < 60)
        {
            timerStyle.normal.textColor = Color.red;
        }
        else
        {
            timerStyle.normal.textColor = Color.white;
        }

        // Calculate the position for the top right corner
        float labelX = Screen.width - 210;
        float labelY = 10;

        // Draw the shadow (optional)
        GUI.Label(new Rect(labelX + 3, labelY + 3, 200, 50), timeText, new GUIStyle(timerStyle) { normal = { textColor = Color.black } });

        // Draw the main timer text
        GUI.Label(new Rect(labelX, labelY, 200, 50), timeText, timerStyle);
    }

    public void QuitGame()
    {
        GameController.instance.ShowFailPanel();
    }
}
