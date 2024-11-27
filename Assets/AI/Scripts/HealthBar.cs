using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBar;
    public float healthAmount = 1.0f;

    public float healthDecrease;

    public bool isPlayer;
    private NavMeshAgent agent;

    public AudioClip hitSound;
    private AudioSource audioSource;

    private void Start()
    {
        healthBar.fillAmount = healthAmount;

        agent = GetComponent<NavMeshAgent>();

        if (agent == null) {
            Debug.Log("Health RB is null");
        }

        audioSource = GetComponent<AudioSource>(); 
    }
    public void ApplyDamage()
    {
        healthAmount -= healthDecrease;
        healthBar.fillAmount = healthAmount;
        if(healthBar.fillAmount <= 0)
        {
            if (isPlayer)
            {
                GameController.instance.ShowFailPanel();
                Destroy(this.gameObject);
            }else
            {
                GameController.instance.OnEnemyDamage?.Invoke();
                Destroy(this.gameObject);
            }
            
        } else {
            PlayEnemySound(hitSound);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Laser")) {
            ApplyDamage();
            ApplyStop();
        }
    }

    private void ApplyStop()
    {
        if (agent != null)
            agent.enabled = false;

        // Re-enable NavMeshAgent after a short delay
        StartCoroutine(ReenableNavMeshAgent());
    }

    private IEnumerator ReenableNavMeshAgent()
    {
        yield return new WaitForSeconds(0.5f); // Adjust the time as needed
        if (agent != null)
            agent.enabled = true;
    }

    private void PlayEnemySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
