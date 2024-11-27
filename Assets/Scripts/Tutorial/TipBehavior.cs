using UnityEngine;

public class TipBehavior : MonoBehaviour
{
    [TextArea] public string tipMessage; // Message to display in the tip popup

    private void Start()
    {
        if (TipManager.instance == null) {
            Debug.LogError("TipManager Missing from Scene!");
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player"))
        {
            TipManager.instance.ShowTip(tipMessage); // Show the tip
            Destroy(this.gameObject);       // Destroy the question mark object
        }
    }
}
