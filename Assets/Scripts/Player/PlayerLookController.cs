using UnityEngine;

//This class is responsible for coordinating the position of mouse and rotating the player object to face that direction
public class PlayerLookController : MonoBehaviour
{
    private Camera playerCamera;
    public float rotationSpeed = 10f;

    void Start()
    {
        playerCamera = Camera.main;
    }

    void Update()
    {
        HandleMouseLook();
    }

    void HandleMouseLook()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            Vector3 direction = (targetPosition - transform.position).normalized;
            direction.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
        }
    }
}
