using UnityEngine;
using System.Collections;

public class GlowOnHit : MonoBehaviour
{
    public Material offMaterial;   // Assign Dull Red Material in Inspector
    public Material onMaterial; // Assign Bright Red Material in Inspector
    private Renderer sphereRenderer;
    public GameObject particleObject; 

    public bool OnOverrride;
    private bool isOn = false;     // Tracks if currently in bright state
    private float brightDuration = 5f; // Duration for bright state

    public AudioClip onSound;
    public AudioClip offSound;
    public AudioClip tickSound;

    void Start()
    {
        sphereRenderer = GetComponent<Renderer>();
        sphereRenderer.material = OnOverrride ? onMaterial : offMaterial;  // Start with dull red

        if (particleObject != null)
        {
            particleObject.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (OnOverrride) {
            return;
        }
        if (other.CompareTag("Laser") && !isOn)
        {
            // Switch to bright material and start timer
            StartCoroutine(EnterOnState());
        }
    }

    private IEnumerator EnterOnState()
    {
        isOn = true;
        sphereRenderer.material = onMaterial;
        if (particleObject != null)
        {
            particleObject.SetActive(true);
            if (onSound != null)
                AudioSource.PlayClipAtPoint(onSound, transform.position);
        }

        GameController.instance.RegisterTriggerOn();

        if (tickSound != null)
            AudioSource.PlayClipAtPoint(tickSound, transform.position);
        
        // Wait for the specified duration
        yield return new WaitForSeconds(brightDuration);
        
        // Switch back to dull material
        if (particleObject != null)
        {
            particleObject.SetActive(false);
            if (offSound != null)
                AudioSource.PlayClipAtPoint(offSound, transform.position);
        }
        sphereRenderer.material = offMaterial;
        isOn = false;
        GameController.instance.RegisterTriggerOff();
    }
}


