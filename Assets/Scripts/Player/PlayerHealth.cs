using UnityEngine;

//This class manages all logic required for player health tracking.
public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    private int currentHealth;

    public GameObject explosionPrefab;
    public GameObject damageEffect1;
    public GameObject damageEffect2;
    public GameObject shieldObject;

    public GameObject shieldHeart;
    public GameObject heart2;
    public GameObject heart3;

    public GameObject playerIndicators;

    public AudioClip damageSound;
    public AudioClip healthSound;
    public AudioClip deathSound;

    private void Start()
    {
        currentHealth = maxHealth - 1; //the player will start of with one less then max health to allow bubble shield at max health

        if (damageEffect1 != null) damageEffect1.SetActive(false);
        if (damageEffect2 != null) damageEffect2.SetActive(false);
        if (shieldObject != null) shieldObject.SetActive(false);
    }

    //Register Player damage and activate effects or destroy player object
    public void TakeDamage(int damage)
    {

        currentHealth -= damage;
        Debug.Log("Player hit! Current health: " + currentHealth);

        if (damageSound != null)
        {
            AudioSource.PlayClipAtPoint(damageSound, transform.position);
        }

        CheckDamageEffects();

        if (currentHealth <= 0)
        {
            Explode();
        }
    }

    //Player has picked up a health item, register this action and check what effects to undo or add
    public bool GainHealth(int health)
    {
        if (currentHealth < maxHealth) {
            currentHealth += health;
            if (healthSound != null)
            {
                AudioSource.PlayClipAtPoint(healthSound, transform.position);
            }    
            Debug.Log("Player healed! Current health: " + currentHealth);
            CheckHealthEffects();
            return true;
        } else {
            return false;
        }
    }

    //Activate or deactivate damage effects based on health
    private void CheckDamageEffects()
    {
        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
            if (shieldHeart != null) {
               shieldHeart.SetActive(false);     
            } 
        }
        
        if (currentHealth <= 2 && damageEffect1 != null)
        {
            damageEffect1.SetActive(true);
            Debug.Log("Damage effect 1 activated!");
            shieldObject.SetActive(false);
            if (heart2 != null) {
               heart2.SetActive(false);     
            } 
        }
        
        if (currentHealth <= 1 && damageEffect2 != null)
        {
            damageEffect2.SetActive(true);
            Debug.Log("Smoke effect 2 activated!");
            if (heart3 != null) {
               heart3.SetActive(false);     
            } 
        }
    }

    //Activate or deactivate damage effects based on health
    private void CheckHealthEffects()
    {
        if (currentHealth == maxHealth && shieldObject != null)
        {
            shieldObject.SetActive(true);
            if (shieldHeart != null) {
               shieldHeart.SetActive(true);     
            } 
        }

        if (currentHealth > 2 && damageEffect1 != null)
        {
            damageEffect1.SetActive(false);
            Debug.Log("Health effect 1 activated!");
            if (heart2 != null) {
               heart2.SetActive(true);     
            } 
        }
        
        if (currentHealth > 1 && damageEffect2 != null)
        {
            damageEffect2.SetActive(false);
            Debug.Log("Health effect 2 activated!");
            if (heart3 != null) {
               heart3.SetActive(true);     
            } 
        }
    }

    //Explode player game object
    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, transform.rotation);

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Destroy(gameObject);

        if (playerIndicators != null) {
            playerIndicators.SetActive(false);
        }

        Debug.Log("Player has exploded!");

        GameController.instance.ShowFailPanel();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("FallBorder")) {
            Explode();
        }
    }
}
