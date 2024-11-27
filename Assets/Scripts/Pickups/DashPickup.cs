using System;
using UnityEngine;

public class DashPickup : MonoBehaviour
{
    [TextArea] public string tipMessage; // Message to display in the pickup popup
    public AudioClip pickupSound;
    public GameObject indicator;
    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            PlayerMovementController pm = c.attachedRigidbody.gameObject.GetComponent<PlayerMovementController>();
            if (pm != null) {
                if (pm.HasDash) {
                    Debug.Log("Already Have Dash");
                    Destroy(this.gameObject);
                } else {
                    pm.HasDash = true;
                    Destroy(this.gameObject);
                    try {
                        GameController.instance.OnPickup?.Invoke();
                    } catch (NullReferenceException e) {
                        Debug.LogError("Missing GameController, empty object with GameController script is in scene:" + e);
                    }

                    if (pickupSound != null) {
                        AudioSource.PlayClipAtPoint(pickupSound, transform.position);
                    }

                    if (!string.IsNullOrEmpty(tipMessage)) {
                        TipManager.instance.ShowTip(tipMessage);
                    }
                    if (indicator != null) {
                        indicator.SetActive(true);
                    }
                }
            }
        }
    }
}

