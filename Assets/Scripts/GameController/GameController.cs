using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    [SerializeField] private GameObject levelComplete;
    [SerializeField] private GameObject levelFail;

    public Action OnEnemyDamage;
    public Action OnPickup;
    public GameObject End;
    [TextArea] public string endTipMessage; // Message to display when obelisk rises
    public int totalEnemies;
    private int enemyKill;

    public int totalPickups;
    private int pickupCount;

    int triggerCount = 0;
    public int RequiredTriggerCount = 3;
    public GameObject triggerTargetObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        OnEnemyDamage += EnemyKillCounter;
        OnPickup += PickupCounter;
    }
    private void OnDestroy()
    {
        OnEnemyDamage -= EnemyKillCounter;
        OnPickup -= PickupCounter;
    }
    public void ShowCompletePanel()
    {
        if (levelComplete != null) {
            levelComplete.SetActive(true);
            Time.timeScale = 0;
        } else {
            SceneManager.LoadScene("MenuScreen");
        }
    }
    public void ShowFailPanel()
    {
        if (levelFail != null) {
            levelFail.SetActive(true);
            Time.timeScale = 0;
        } else {
            StartCoroutine(DelayEnd());
        }
    }
    private void EnemyKillCounter()
    {
        enemyKill++;
        CheckEnd();
    }

    private void PickupCounter()
    {
        pickupCount++;
        CheckEnd();
    }

    private void CheckEnd() {
        if(enemyKill >= totalEnemies && pickupCount >= totalPickups)
        {
            if (End != null) {
                End.SetActive(true);
                if (TipManager.instance != null) {
                    if (!string.IsNullOrEmpty(endTipMessage)) {
                        TipManager.instance.ShowTip(endTipMessage);
                    }
                }
            } else {
                ShowCompletePanel();
            }
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    private IEnumerator DelayEnd()
    {
        // Wait for 3 seconds so player can see themselves explode
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void RegisterTriggerOn()
    {
        triggerCount++;
        CheckBrightCount();
    }

    public void RegisterTriggerOff()
    {
        triggerCount--;
    }

    private void CheckBrightCount()
    {
        // Activate the target object if all spheres are bright
        if (SceneManager.GetActiveScene().name.Equals("LevelTutorial")) {
            if (triggerCount >= RequiredTriggerCount && triggerTargetObject != null)
            {
                var targetAnimator = triggerTargetObject.GetComponent<Animator>();

                if (targetAnimator != null)
                {
                    targetAnimator.SetTrigger("Door");
                }
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("LevelOne")) {
            if (triggerCount >= RequiredTriggerCount && triggerTargetObject != null)
            {
                triggerTargetObject.SetActive(true);
            }
        }

        if (SceneManager.GetActiveScene().name.Equals("LevelThree")) {
            if (triggerCount >= RequiredTriggerCount && triggerTargetObject != null)
            {
                var targetAnimator = triggerTargetObject.GetComponent<Animator>();

                if (targetAnimator != null)
                {
                    targetAnimator.SetTrigger("Door");
                }
            }
        }
    }
}
