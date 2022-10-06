using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rTouchPos = OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch);
        Quaternion rTouchRot = OVRInput.GetLocalControllerRotation(OVRInput.Controller.RTouch);

        transform.localPosition = rTouchPos;
        transform.localRotation = rTouchRot;
    }
}
