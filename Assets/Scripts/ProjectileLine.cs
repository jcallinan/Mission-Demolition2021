using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; // singleton

    [Header("Set In Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake()
    {
        S = this; // set the singleton
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }
    public GameObject poi
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                // when _poi is set to something new, it resets everything
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
                    
            }
        }
    }
    public void Clear()
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }
    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist)
        {
            return;
        }
        if (points.Count == 0)
        { // if this is the launch point
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // set the first two points
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        } else
        {
            //normal behavior of adding a point
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count -1, lastPoint);
            line.enabled = true;
        }
    }
    public Vector3 lastPoint
    {
        get { 
        if (points == null)
            {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

 
    void FixedUpdate()
    {
        if ( poi == null)
        {
            // if there is no poi, search for one
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile")
                {
                    poi = FollowCam.POI;
                } else
                {
                    return;
                }
            } else
            {
                return;
            }
        }
        // if there is a POI, it's loc is added every FixedUpdate
        AddPoint();
        if (FollowCam.POI == null)
        {
            poi = null;
        }


    }
}
