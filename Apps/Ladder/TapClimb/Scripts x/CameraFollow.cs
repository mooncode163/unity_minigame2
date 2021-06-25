using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    /// <summary>
    /// 
    /// x=-2.1 / y=-2.49
    /// x=-1.83 / y=-2.16
    /// </summary>
    /// 

    public Transform follow;
    //public Vector3 offset;
    Vector3 velocity;
    public float smoothFactor;

    public Transform rHand, lHand;
    public float x, z,y;
    //public Animator anim;

    void Start()
    {
        //offset = transform.position - bulletFollow.position;
    }

    void Update()
    {
        //Vector3 follow = bulletFollow.position + offset;

        Vector3 foll = new Vector3(x, (rHand.position.y + lHand.position.y) / 2 + y, z);

        transform.position = Vector3.SmoothDamp(transform.position, foll, ref velocity, smoothFactor);
    }
}
