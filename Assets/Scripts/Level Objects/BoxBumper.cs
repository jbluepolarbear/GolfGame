using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBumper : Bumper
{
    protected override Vector3 GetForce(Vector3 otherPosition)
    {
        return base.GetForce(otherPosition);
        // Vector3 vecA = new Vector3(otherPosition.x, otherPosition.y, 0.0f);
        // Vector3 vecB = new Vector3(transform.position.x, transform.position.y, 0.0f);
        // Vector3 direction = (vecA - vecB).normalized;
        // if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        // {
        //     if (direction.x > 0.0f)
        //     {
        //         direction = new Vector3(1.0f, 0.0f, 0.0f);
        //     }
        //     else
        //     {
        //         direction = new Vector3(-1.0f, 0.0f, 0.0f);
        //     }
        // }
        // else
        // {
        //     if (direction.y > 0.0f)
        //     {
        //         direction = new Vector3(0.0f, 1.0f, 0.0f);
        //     }
        //     else
        //     {
        //         direction = new Vector3(0.0f, -1.0f, 0.0f);
        //     }
        // }
        // return direction * _thrust;
    }
}
