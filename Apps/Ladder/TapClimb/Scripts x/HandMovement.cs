using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandMovement : MonoBehaviour
{
    float stepDistance = 2.0f;

    public Animator anim;

    public LayerMask stepLayer;
    Vector3 lastHandPos;

    public PlayerMovement movement;

    [HideInInspector] public bool check = true;

    float moveTime = 0.09f;

    public bool isRightHand;

    public ParticleSystem dustEffect;

    public Transform circlePos;

    public Outline outline;

    public bool stopped = false;

    IEnumerator waitToCheck = null;

    private void Start()
    {
        lastHandPos = transform.position;
    }
    public void MoveOne()
    {
        //transform.position = transform.position + (Vector3.up * step);
        transform.DOComplete();
        CheckStepCollision(0);
        transform.DOMoveY(transform.position.y + stepDistance, moveTime);
        anim.SetTrigger("OpenAndClose");
        CheckStepCollision(moveTime);
    }

    public void MoveHalfOne(int up)
    {
        //transform.position = transform.position + (Vector3.up * step);
        transform.DOComplete();
        CheckStepCollision(0);
        transform.DOMoveY(transform.position.y + (stepDistance/2*up), moveTime*4);
        anim.SetTrigger("OpenAndClose");
        CheckStepCollision(moveTime*4,true);
    }

    public void SetStepDistance(float step,float yPos)
    {
        this.stepDistance = step;
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }

    public void CheckStepCollision(float time, bool checkOtherHand = false)
    {
        if (waitToCheck!=null) StopCoroutine(waitToCheck);
        waitToCheck = waitToCheckStepCollision(time, checkOtherHand);
        StartCoroutine(waitToCheck);
    }

    IEnumerator waitToCheckStepCollision(float time,bool checkOtherHand = false)
    {
        check = false;
        yield return new WaitForSeconds(time);
        if (Physics.OverlapSphere(GetPos(), .2f, stepLayer).Length > 0)
        {
            
            if (!checkOtherHand)
            {
                //Debug.Log("CHECKED");
                check = true;
                lastHandPos = transform.position;
                movement.OneStepMoved();
                StartCoroutine(playDustEffect(.2f,true,true));
            }
            else
            {
                if (Physics.OverlapSphere(movement.rHand.GetPos(), .2f, stepLayer).Length > 0
                    && Physics.OverlapSphere(movement.lHand.GetPos(), .2f, stepLayer).Length > 0)
                {
                    //Debug.Log("CHECKED");
                    check = true;
                    lastHandPos = transform.position;
                    StartCoroutine(playDustEffect(.0f,true,false));
                }
                else
                {
                    //Debug.Log("NOT CHECKED");
                    check = false;
                    if (movement.FaultGetHearts() > 0) ReturnToLastPos(isRightHand);
                    //movement.Fall();
                }
            }
            
        }
        else
        {
            check = false;
            //Debug.Log("NOT CHECKED");
            if (checkOtherHand)
            {
                if(movement.FaultGetHearts()>0) ReturnToLastPos(isRightHand);

                //movement.Fall();
            }
            else
            {
                if (movement.FaultGetHearts() > 0) ReturnToLastPos();
                //movement.Fall();
            }
        }

        if (check)
        {
            Collider[] col = Physics.OverlapSphere(GetPos(), .2f, stepLayer);
            col[0].GetComponentInParent<StepSc>().StopAnimation();
        }
    }

    public void ReturnToLastPos(bool switchNextHand=true)
    {
        StartCoroutine(stopForAnAmountOfTime());
        movement.HandReturnedToLastPos(switchNextHand);
        anim.SetTrigger("DidntCatch");
        transform.DOComplete();
        transform.DOMoveY(lastHandPos.y, moveTime*4);
        //CheckStepCollision(moveTime*4);
        movement.combo.ResetCombo();
    }

    IEnumerator stopForAnAmountOfTime()
    {
        stopped = true;
        yield return new WaitForSeconds(1.0f);
        stopped = false;
    }

    public Vector3 GetPos()
    {
        if (isRightHand) return new Vector3(.47f, transform.position.y, 0);
        else return new Vector3(-.47f, transform.position.y, 0);
    }

    IEnumerator playDustEffect(float wait, bool vibrate,bool sound)
    {
        yield return new WaitForSeconds(wait);
        dustEffect.Play();
        if (vibrate)
        {
            Vibration.Vibrate(35);
        }
        if (sound)
        {
            movement.gameManager.audioManager.PlayCatch();
        }
        
    }

    public void SetOutline(bool active)
    {
        if (active)
        {
            StopCoroutine("DecreaseOutlineWidthOverTime");
            StartCoroutine("increaseOutlineWidthOverTime");
        }
        else
        {
            StopCoroutine("increaseOutlineWidthOverTime");
            StartCoroutine("DecreaseOutlineWidthOverTime");
        }
    }

    IEnumerator increaseOutlineWidthOverTime()
    {
        float increament = 6.97f / (.3f / Time.deltaTime);
        while (outline.OutlineWidth < 6.97f)
        {
            outline.OutlineWidth = outline.OutlineWidth + increament;
            yield return new WaitForEndOfFrame();
        }
        outline.OutlineWidth = 6.97f;
    }

    IEnumerator DecreaseOutlineWidthOverTime()
    {
        float increament = 6.97f / (.1f / Time.deltaTime);
        while (outline.OutlineWidth > 0.01f)
        {
            outline.OutlineWidth = outline.OutlineWidth - increament;
            yield return new WaitForEndOfFrame();
        }
        outline.OutlineWidth = 0;
    }
}
