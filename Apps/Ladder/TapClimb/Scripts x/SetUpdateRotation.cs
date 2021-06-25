using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpdateRotation : MonoBehaviour
{
    public Transform follow;

    // Update is called once per frame
    void Update()
    {
        transform.rotation = follow.rotation;
    }
}
