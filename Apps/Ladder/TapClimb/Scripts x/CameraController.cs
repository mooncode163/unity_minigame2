using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Animator anim;

    public void DidntCatch()
    {
        anim.SetTrigger("DidntCatch");
    }

    public void Fall()
    {
        anim.SetTrigger("Fall");
    }


    public void SetSide(int side)
    {
        anim.SetInteger("Side", side);
    }

    public void CamStart()
    {
        anim.SetBool("Start",true);
    }

    public void SetCombo(bool active)
    {
        anim.SetBool("Combo", active);
    }

    public void Shoping(bool active)
    {
        anim.SetBool("Shoping", active);
    }
}
