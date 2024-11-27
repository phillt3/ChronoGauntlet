using System;
using UnityEngine;

public class FireRatePickup : MonoBehaviour
{
    [TextArea] public string tipMessage; // Message to display in the pickup popup
    public GameObject indicator;
    public AudioClip pickupSound;
    void OnTriggerEnter(Collider c) {
        if (c.gameObject.CompareTag("Player")) {
            Debug.Log("Raygun hit");
            RayGun rg = c.gameObject.GetComponentInChildren<RayGun>();
            
            if (rg != null) {
                rg.shootRate = 0.50f;
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

