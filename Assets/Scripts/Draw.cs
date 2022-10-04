using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Transform pen;
    public Texture2D canvas;
    public Color color;
    public int brushSize;
    public PenTip penTip;
    private Color eraseColor;
    private Renderer renderer;
    private bool useOldCode;
    // private LineRenderer lr;
    public List<Vector3> drawPoints;
    private int count;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lr = gameObject.AddComponent<LineRenderer>();
        canvas = new Texture2D(384, 256);
        eraseColor = canvas.GetPixel(0, 0);
        renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = canvas;
        useOldCode = false;
        lr.startColor = Color.red;
        lr.endColor = Color.red;
        lr.startWidth = 0.01f;
        lr.endWidth = 0.01f;
        drawPoints = new List<Vector3> { penTip.transform.position };
        lr.SetPositions(drawPoints.ToArray());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;
        Vector2 hitCoords = new Vector2();

        if (!useOldCode)
        {
            // Debug.Log(penTip.GetisTouchingTablet());
            // new code
            // if (penTip.GetisTouchingTablet())
            // {
                if (OVRInput.Get(OVRInput.RawButton.X))
                {
                    drawAt(penTip.GetcollisionCoords());
                    Debug.Log(penTip.GetcollisionCoords());
                }
            if (count % 24 == 0) {
                LineRenderer lr = GetComponent<LineRenderer>();
                Debug.Log("hi" + drawPoints.Count);
                Debug.Log(drawPoints[drawPoints.Count - 1] + " | " + penTip.transform.position);
                if (drawPoints[drawPoints.Count - 1] != penTip.transform.position)
                {
                    drawPoints.Add(penTip.transform.position);
                    lr.positionCount++;
                    lr.SetPosition(drawPoints.Count - 1, penTip.transform.position);
                }
                // Debug.Log(drawPoints.Count);
            }
            count++;
            // }
        }

        // backup code
        else
        {
            if (Physics.Raycast(pen.position, pen.up * -1, out hit, Mathf.Infinity))
            {
                hitCoords = transform.InverseTransformPoint(hit.point);

                LineRenderer lineRenderer = GetComponent<LineRenderer>();
                Vector3[] positions = { pen.position, hit.point };
                Debug.Log(positions);
                lineRenderer.startColor = Color.red;
                lineRenderer.endColor = Color.red;
                lineRenderer.startWidth = 0.001f;
                lineRenderer.endWidth = 0.001f;
                lineRenderer.SetPositions(positions);

                // draw
                if (OVRInput.Get(OVRInput.RawButton.RIndexTrigger))
                {
                    drawAt(hitCoords);
                }

                // erase
                if (OVRInput.Get(OVRInput.RawButton.RHandTrigger))
                {
                    eraseAt(hitCoords);
                }
            }
        }
        
    }

    void drawAt(Vector2 hitCoords)
    {
        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                canvas.SetPixel((int)(((hitCoords.x + 0.5) * -384) + i), (int)(((hitCoords.y + 0.5) * -256)) + j, color);
            }
        }
        canvas.Apply();
    }

    void eraseAt(Vector2 hitCoords)
    {
        for (int i = -brushSize; i < brushSize; i++)
        {
            for (int j = -brushSize; j < brushSize; j++)
            {
                canvas.SetPixel((int)(((hitCoords.x + 0.5) * -384) + i), (int)(((hitCoords.y + 0.5) * -256)) + j, eraseColor);
            }
        }
        canvas.Apply();
    }
}
