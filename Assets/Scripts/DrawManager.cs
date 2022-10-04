using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    public GameObject drawPrefab;   // the prefab that has the component for a single line stroke
    public GameObject tabletObj;    // the tablet object
    public Transform penTip;        // the object/transform for tip of the pen
    int framePause;                 // a variable to determine when to sample a point
    int framePauseFactor;           // The higher the factor, the lower the rate of sampling.
    List<Vector2> origins;          // Each point's origin with respect to tablet origin on the plane
    GameObject theTrail;            // a single stroke with an instance of the drawPrefab

    void Start()
    {
        // creates a reference trail object based on a new, cloned instance of drawPrefab
        theTrail = (GameObject) Instantiate(drawPrefab, this.transform.position, Quaternion.identity);
        framePauseFactor = 10;
    }

    void FixedUpdate()
    {
        RaycastHit hit;             // the result of the raycast

        // Here, we want to cast a ray from the tip of the pen to determine where to draw.
        // If it hits the canvas (implicitly, otherwise it just hits nothing but empty space),
        // we'll draw We store the result in hit.
        if (Physics.Raycast(penTip.position, penTip.up * -1, out hit, Mathf.Infinity))
        {
            // lowers the amount of sample points
            if (framePause == 0)
            {
                LineRenderer lr = theTrail.GetComponent<LineRenderer>();

                // prevents the line renderer from adding the same point over and over;
                // only add if last added vertex/position isn't the same as where it's being
                // hit by the raycast
                if (lr.GetPosition(lr.positionCount - 1) != hit.point)
                {
                    // add point
                    lr.positionCount++;
                    lr.SetPosition(lr.positionCount - 1, hit.point);
                }
            }
            // lowers the amount of sample points
            framePause = (framePause + 1) % framePauseFactor;
        }
    }
}
