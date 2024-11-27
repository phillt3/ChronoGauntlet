using UnityEngine;

public class PlayerIndicators : MonoBehaviour
{
    // Reference to the player object
    public Transform player;

    // Offset above the player
    public Vector3 offset = new Vector3(0, 5, 0); // Adjust Y to set height above the player

    void LateUpdate()
    {
        if (player != null)
        {
            // Set the position of the object to always follow above the player
            transform.position = player.position + offset;
        }
    }
}
