using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;  // the static point of interest

    [Header("Set in Inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Set Dynamically")]
    public float camZ; // the desired Z pos of the camera

    private void Awake()
    {
        camZ = this.transform.position.z;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        //limited to 50 fps
        // UI elements, not action
        //  if (POI == null) return;
        //get the position of the poi
        //  Vector3 destination = POI.transform.position;
        Vector3 destination;
        if (POI == null)
        {
            destination = Vector3.zero;
        } else
        {
            destination = POI.transform.position;
            if (POI.tag == "Projectile")
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping())
                {
                    POI = null;
                    return;
                }
            }
        }

        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);
        destination = Vector3.Lerp(transform.position, destination, easing);
        // force destination.z  to be camZ to keep the camera far enough away
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10;

    }
}
