using UnityEngine;

//This class is responsible for making sure the muzzle component of the player model (attached to the head)
//always follows the user's mouse pointer.
public class MuzzleFollowPointer : MonoBehaviour
{
    private Camera playerCamera;
    private LineRenderer lr;

    void Start()
    {
        playerCamera = Camera.main;
        lr = GetComponent<LineRenderer>();
    }

   void Update()
    { 
        //prevent laser from moving while in pause mode
        if (Time.timeScale == 0f) {
            return;
        }
        Vector3 mousePosition = Input.mousePosition;

        Ray ray = playerCamera.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 targetPosition = hit.point;

            Vector3 directionToTarget = targetPosition - transform.position;

            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, 0);
            transform.rotation = targetRotation;

            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit sighthit)) {
                lr.SetPosition(0, transform.position);
                lr.SetPosition(1, sighthit.point);
            }
        }
    }
}
