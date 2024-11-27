using UnityEngine;

public class LaserShotBehavior : MonoBehaviour
{
    public Vector3 m_target; //point of mouse
    public GameObject collisionExplosion; //explosion effect to occur at certain range or when object hit
    public float speed; //speed of laser
    public float maxDistance = 10f; //how far laser can travel
    public AudioClip explosionSound;

    private Vector3 startPosition; //start position of laser (muzzle)

    void Start()
    {
        startPosition = transform.position;
        Vector3 direction = (m_target - startPosition).normalized;
        float distanceToTarget = Vector3.Distance(startPosition, m_target);

        if (distanceToTarget > maxDistance)
        {
            //target accounting for max distance laser can travel
            m_target = startPosition + direction * maxDistance;
        }
    }

    void Update()
    {
        float step = speed * Time.deltaTime;

        if (m_target != null)
        {
            if (transform.position == m_target)
            {
                //have reached target so explote
                explode();
                return;
            }
            //continue towards target
            transform.position = Vector3.MoveTowards(transform.position, m_target, step);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        explode();
    }

    void OnTriggerEnter(Collider other)
    {
        explode();
    }

    public void setTarget(Vector3 target)
    {
        m_target = target;
    }

    //Set off explode animation and sound and destroy both laser and explosion
    void explode()
    {
        if (collisionExplosion != null)
        {
            GameObject explosion = Instantiate(
                collisionExplosion, transform.position, transform.rotation);

            if (explosionSound != null)
            {
                AudioSource.PlayClipAtPoint(explosionSound, transform.position);
            }

            Destroy(gameObject);
            Destroy(explosion, 1f);
        }
    }
}
