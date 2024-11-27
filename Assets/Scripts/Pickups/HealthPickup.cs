using System;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [TextArea] public string tipMessage; // Message to display in the pickup popup
    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            PlayerHealth ph = c.attachedRigidbody.gameObject.GetComponent<PlayerHealth>();
            if (ph != null) {
                if (ph.GainHealth(1)) {
                    try {
                        GameController.instance.OnPickup?.Invoke();
                    } catch (NullReferenceException e) {
                        Debug.LogError("Missing GameController, empty object with GameController script is in scene:" + e);
                    }
                    Destroy(this.gameObject);
                    if (!string.IsNullOrEmpty(tipMessage)) {
                        TipManager.instance.ShowTip(tipMessage);
                    }
                } else {
                    Debug.Log("Health already full.");
                }
            }
        }
    }
}

