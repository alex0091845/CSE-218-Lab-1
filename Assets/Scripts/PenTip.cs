using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenTip : MonoBehaviour
{
    private bool isTouchingTablet1;
    private Vector3 collisionCoords1;

    public Vector3 GetcollisionCoords()
    {
        return collisionCoords1;
    }

    private void SetcollisionCoords(Vector3 value)
    {
        collisionCoords1 = value;
    }

    public bool GetisTouchingTablet()
    {
        return isTouchingTablet1;
    }

    private void SetisTouchingTablet(bool value)
    {
        isTouchingTablet1 = value;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.name == "Tablet")
        {
            // Debug.Log("collide");
            SetisTouchingTablet(true);
            SetcollisionCoords(collision.GetContact(0).point);
        }
    }
}
