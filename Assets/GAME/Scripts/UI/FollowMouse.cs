using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    void Update()
    {
        Plane plane = new Plane(Vector3.forward, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (plane.Raycast(ray, out distance))
        {
            Vector3 hitPoint = ray.GetPoint(distance);  
            transform.position = new Vector3(hitPoint.x, hitPoint.y, 0);
        }
    }
}
