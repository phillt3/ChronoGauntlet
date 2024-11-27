using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            if (SceneManager.GetActiveScene().name.Equals("LevelTutorial")) {
                SceneManager.LoadScene("LevelOne");
                return;
            } else if (SceneManager.GetActiveScene().name.Equals("LevelOne")) {
                SceneManager.LoadScene("LevelTwo");
                return;
            } else if (SceneManager.GetActiveScene().name.Equals("LevelTwo")) {
                SceneManager.LoadScene("LevelThree");
                return;
            } else if (SceneManager.GetActiveScene().name.Equals("LevelThree")) {
                SceneManager.LoadScene("GameCompleteScreen");
                return;
            }
            QuitGame();
        }
    }

    public void QuitGame()
    {
        GameController.instance.ShowCompletePanel();
    }
}
