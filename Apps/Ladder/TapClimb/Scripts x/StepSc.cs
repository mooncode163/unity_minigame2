using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSc : MonoBehaviour
{
    public enum StepType {Long,Left,Right,LeftOrRight}
    public StepType stepType;
    public Animator anim;
    public int animType=0;
    private void Start()
    {
        if(anim.isActiveAndEnabled) anim.SetInteger("Type", animType);
    }

    public void StopAnimation()
    {
        anim.speed = 0;
    }

}
