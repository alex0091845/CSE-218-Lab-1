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
    GameObject theTrail;            // a single stroke with an instance of the drawPrefab
    LineRenderer rr;                // pen ray renderer
    bool enableDraw = false;
    bool enableErase = false;
    RaycastHit hit;

    void Start()
    {
        // creates a reference trail object based on a new, cloned instance of drawPrefab
        framePauseFactor = 2;
        rr = gameObject.GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        // DRAW
        // Here, we want to cast a ray from the tip of the pen to determine where to draw.
        // If it hits the canvas (implicitly, otherwise it just hits nothing but empty space),
        // we'll draw We store the result in hit.
        if (Physics.Raycast(penTip.position, penTip.up * -1, out hit, Mathf.Infinity))
        {
            // cast a ray from pen to board
            rr.SetPositions(new Vector3[2] { penTip.position, hit.point });

            // upon activating draw
            if (!enableDraw && (OVRInput.GetDown(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A)))
            {
                enableDraw = true;
                enableErase = false;

                // creates a reference trail object based on a new, cloned instance of drawPrefab
                theTrail = Instantiate(drawPrefab, tabletObj.transform.position, tabletObj.transform.localRotation);
                theTrail.transform.SetParent(tabletObj.transform);
                theTrail.transform.localScale = new Vector3(1, 1, 1);
                theTrail.GetComponent<LineRenderer>().SetPosition(0, tabletObj.transform.InverseTransformPoint(hit.point + hit.normal * 0.005f));
            }
            // while drawing and not switched to erase mode
            else if (enableDraw && !enableErase && (OVRInput.Get(OVRInput.RawButton.A) || Input.GetKeyDown(KeyCode.A)))
            {
                if (framePause == 0)
                {
                    LineRenderer lr = theTrail.GetComponent<LineRenderer>();

                    // prevents the line renderer from adding the same point over and over;
                    // only add if last added vertex/position isn't the same as where it's being
                    // hit by the raycast
                    if (lr.GetPosition(lr.positionCount - 1) != tabletObj.transform.InverseTransformPoint(hit.point + hit.normal * 0.005f))
                    {
                        // add point
                        lr.positionCount++;
                        lr.SetPosition(lr.positionCount - 1, tabletObj.transform.InverseTransformPoint(hit.point + hit.normal * 0.005f));
                    }
                }
            }
            // upon cancelling draw or switching over to erase mode
            else if ((enableDraw && (OVRInput.GetUp(OVRInput.RawButton.A) || Input.GetKeyUp(KeyCode.A))) || enableErase)
            {
                enableDraw = false;
            }

            // ERASE
            // upon activating erase
            if (OVRInput.GetDown(OVRInput.RawButton.B) && !enableErase)
            {
                enableErase = true;
                enableDraw = false;

                // need all lines to be mesh colliders to be detected by raycast
                // and then destroyed
                TurnAllLinesIntoMesh();
            }
            // while erasing and not switched to draw mode
            else if (enableErase && !enableDraw && OVRInput.Get(OVRInput.RawButton.B))
            {
                Erase(hit);
            }
            // upon cancelling erase or switching over to draw mode
            else if ((enableErase && OVRInput.GetUp(OVRInput.RawButton.B)) || enableDraw)
            {
                enableErase = false;
            }

            // lowers the amount of sample points
            framePause = (framePause + 1) % framePauseFactor;
        }
        // implementation choice: stop draw mode if raycast is not hitting the 
        // canvas.
        else
        {
            enableDraw = false;
            rr.SetPositions(new Vector3[2] { Vector3.zero, Vector3.zero });
        }
    }

    void TurnAllLinesIntoMesh()
    {
        // get all children
        foreach (Transform child in tabletObj.transform.GetComponentsInChildren<Transform>())
        {
            // find mesh component
            MeshCollider mc = child.GetComponent<MeshCollider>();

            // if no mesh, create one
            if (mc == null)
            {
                mc = child.gameObject.AddComponent<MeshCollider>();
                Mesh mesh = new Mesh();
                LineRenderer line = child.GetComponent<LineRenderer>();
                line.BakeMesh(mesh);
                mc.sharedMesh = mesh;
            }

        }
    }

    // destroy the whole stroke if line is detected by raycast
    void Erase(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "Line")
        {
            Destroy(hit.transform.gameObject);
        }
    }
}
