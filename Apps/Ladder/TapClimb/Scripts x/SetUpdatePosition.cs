﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpdatePosition : MonoBehaviour
{
    public Transform follow;

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position;
    }
}
