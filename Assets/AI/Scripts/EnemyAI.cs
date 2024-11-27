using System.Collections;
using UnityEngine;
using UnityEngine.AI;  // For NavMeshAgent (if you're using Unity's navigation system)

public class EnemyAI : MonoBehaviour
{
    public Transform player;               // The player to follow
    public float followRange = 15f;        // The distance within which the AI will start following the player
    public float attackRange = 2f;         // The distance within which the AI will attack the player
    public float attackCooldown = 2f;      // Time between attacks
    public int attackDamage = 1;

    private NavMeshAgent navMeshAgent;     // For movement (requires a NavMeshAgent component)
    private Animator animator;             // To trigger attack animations
    private float lastAttackTime = 0f;     // To track when the AI last attacked

    public AudioClip attackSound;
    public AudioClip chatterSound;
    private AudioSource audioSource;



    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();  // Get the NavMeshAgent component
        animator = GetComponent<Animator>();          // Get the Animator component
        audioSource = GetComponent<AudioSource>(); 
    }

    void Update()
    {
       if(player != null && navMeshAgent.enabled)
        {
            float distanceToPlayer = Vector3.Distance(player.position, transform.position);

            if (distanceToPlayer <= attackRange)
            {
                // Attack the player if within attack range
                AttackPlayer();
            }
            else if (distanceToPlayer <= followRange)
            {
                // Follow the player if within follow range but outside attack range
                FollowPlayer();
                animator.SetBool("Attack", false);
            }
            else
            {
                // If the player is outside the follow range, the AI stops moving
                StopFollowing();
            }
        }
    }

    private void PlayEnemySound(AudioClip clip)
    {
        if (clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void FollowPlayer()
    {
        if (navMeshAgent.isStopped == true) {
            PlayEnemySound(chatterSound);
        }
        // Set destination for the NavMeshAgent to follow the player
        navMeshAgent.isStopped = false;
        navMeshAgent.SetDestination(player.position);

        // Update the animator to reflect that the enemy is moving (optional, depending on animation setup)
        //animator.SetBool("isMoving", true);
    }

    void StopFollowing()
    {
        // Stop the NavMeshAgent from moving
        navMeshAgent.isStopped = true;

        // Update the animator to reflect that the enemy is idle (optional)
        //animator.SetBool("isMoving", false);
    }

    void AttackPlayer()
    {
        // Stop the AI from moving to the player while attacking
        navMeshAgent.isStopped = true;

        // Check if enough time has passed since the last attack
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            // Trigger the attack animation
            animator.SetBool("Attack", true);
            PlayEnemySound(attackSound);
            print("Decrease Player health");

            GameObject player = GameObject.FindWithTag("Player");
            if(player != null)
            {
                //player.GetComponent<HealthBar>().ApplyDamage();
                player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);
            }

            // Call the damage or attack method here (optional: Implement damage system)
            // Example: player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);

            // Reset the last attack time
            lastAttackTime = Time.time;


        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if(navMeshAgent.enabled) {
                navMeshAgent.speed = 3f;
                navMeshAgent.angularSpeed = 100f;
                navMeshAgent.acceleration = 4f;
            }
        }
    }

    //Reset player movement after exiting water
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
        {
            if(navMeshAgent.enabled) {
                if(gameObject.name.Contains("Slime")) {
                    navMeshAgent.speed = 4f;
                    navMeshAgent.angularSpeed = 120f;
                    navMeshAgent.acceleration = 8f;
                } else {
                    navMeshAgent.speed = 5f;
                    navMeshAgent.angularSpeed = 120f;
                    navMeshAgent.acceleration = 8f;
                }
            }
        }
    }
}
