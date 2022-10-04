using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    public OVRInput.Controller lController;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 lTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Quaternion lTouchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        transform.localPosition = lTouchPos;
        transform.localRotation = lTouchRot;
    }
}
