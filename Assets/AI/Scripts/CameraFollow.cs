using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;                  // Reference to the player's transform
    public Vector3 offset = new Vector3(0f, 5f, -7f);  // Offset for the camera (behind and above the player)
    public float smoothSpeed = 0.125f;        // Smooth speed for following the player
    public float rotationSpeed = 100f;        // Speed at which the camera rotates around the player

    private float currentYaw = 0f;            // Yaw rotation of the camera
    private float currentPitch = 0f;          // Pitch rotation of the camera
    public float pitchMin = -20f;             // Minimum pitch angle (downward limit)
    public float pitchMax = 60f;              // Maximum pitch angle (upward limit)

    void LateUpdate()
    {
        if (player != null)
        {
            //// Rotate the camera based on input (mouse or joystick)
            //float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            //float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

            //// Update yaw (horizontal rotation) based on player input
            //currentYaw += horizontalInput;

            //// Update pitch (vertical rotation) based on player input, clamped between pitchMin and pitchMax
            //currentPitch -= verticalInput;
            currentPitch = Mathf.Clamp(currentPitch, pitchMin, pitchMax);

            // Calculate the desired position behind the player based on the offset
            Vector3 desiredPosition = player.position + Quaternion.Euler(currentPitch, currentYaw, 0) * offset;

            // Smoothly move the camera to the desired position
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            // Always have the camera look at the player
            transform.LookAt(player.position + Vector3.up * 2f);  // Adjusting the look point to be slightly above the player
        }
    }
}
