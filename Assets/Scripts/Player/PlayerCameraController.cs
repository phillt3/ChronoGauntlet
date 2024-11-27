using UnityEngine;

//This class is responsible for coordinating the position of the camera above the player.
//The camera is also important for calculating where on the screen the player is clicking.
public class PlayerCameraController : MonoBehaviour
{
    private Transform player;
    public Vector3 offset;
    public float smoothSpeed = 0.125f;

    void Start()
{
    if (player == null)
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    if (player != null)
    {
        transform.position = player.position + offset;
    }
}

    void LateUpdate()
    {
        if (player == null) return;
        
        Vector3 desiredPosition = player.position + offset;
        
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
}
