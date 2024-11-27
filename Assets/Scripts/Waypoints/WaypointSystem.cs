using UnityEngine;
using System.Collections.Generic;

public class WaypointSystem : MonoBehaviour
{
    public GameObject enemyWaypointPrefab;
    public GameObject pickupWaypointPrefab;
    public GameObject endWaypointPrefab;
    public float screenBorderOffset = 50f; // Distance from the screen border

    private List<GameObject> activeWaypoints = new List<GameObject>();

    void Update()
    {
        // Clear any existing waypoints
        foreach (var waypoint in activeWaypoints)
        {
            Destroy(waypoint);
        }
        activeWaypoints.Clear();

        // Find all pickups and enemies in the scene
        GameObject[] pickups = GameObject.FindGameObjectsWithTag("Pickup");
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] obelisks = GameObject.FindGameObjectsWithTag("Obelisk");

        foreach (GameObject target in pickups)
        {
            CreateWaypoint(target, pickupWaypointPrefab);
        }
        
        foreach (GameObject target in enemies)
        {
            CreateWaypoint(target, enemyWaypointPrefab);
        }

        foreach (GameObject target in obelisks)
        {
            if(target.activeSelf) {
                CreateWaypoint(target, endWaypointPrefab);
            }
        }
    }

    private void CreateWaypoint(GameObject target, GameObject waypointPrefab)
    {
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(target.transform.position);

        //Check if the target is out of bounds
        if (screenPosition.x < 0 || screenPosition.x > Screen.width || 
            screenPosition.y < 0 || screenPosition.y > Screen.height)
        {
            if (screenPosition.z < 0) {
                screenPosition.x = Mathf.Clamp(screenPosition.x * -1, screenBorderOffset, Screen.width - screenBorderOffset);
                screenPosition.y = Mathf.Clamp(screenPosition.y * -1, screenBorderOffset, Screen.height - screenBorderOffset);
            } else {
                // Clamp position to screen edges
                screenPosition.x = Mathf.Clamp(screenPosition.x, screenBorderOffset, Screen.width - screenBorderOffset);
                screenPosition.y = Mathf.Clamp(screenPosition.y, screenBorderOffset, Screen.height - screenBorderOffset);
            }
            // Create and position the waypoint indicator
            GameObject waypoint = Instantiate(waypointPrefab, screenPosition, Quaternion.identity, transform);

            // Set rotation based on screen position to face outward
            if (screenPosition.y >= Screen.height - screenBorderOffset)       // Top border
            {     
                waypoint.transform.rotation = Quaternion.Euler(0, 0, 90);     // Face up
            }
            else if (screenPosition.y <= screenBorderOffset)                  // Bottom border
            {
                waypoint.transform.rotation = Quaternion.Euler(0, 0, -90);   // Face down
            }
            else if (screenPosition.x <= screenBorderOffset)                  // Left border
            {
                waypoint.transform.rotation = Quaternion.Euler(0, 0, 180);    // Face left
            }
            else if (screenPosition.x >= Screen.width - screenBorderOffset)   // Right border
            {
                waypoint.transform.rotation = Quaternion.Euler(0, 0, 0);   // Face right
            }

            activeWaypoints.Add(waypoint);
        }
    }
}