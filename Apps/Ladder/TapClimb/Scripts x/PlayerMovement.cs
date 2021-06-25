using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    public HandMovement rHand, lHand;
    HandMovement nextHandStep, lastHandStep;
    public Transform camPos;
    float stepDistance = 2.0f;
    public CameraController cam;
    [HideInInspector] public bool fell = false;
    [HideInInspector] public int stepPassed = 2;
    public LadderGenerator ladderGenerator;
    [HideInInspector] public bool control = false;
    public ComboSc combo;

    Vector3 touchPos;
    float minSwipeDis,maxHeight;
    float tapTime;

    public GameManager gameManager;

    public ScoreSc score;

    public int hearts = 2;

    public ParticleSystem windFx;
    ParticleSystem.EmissionModule windFx_Emission;

    private void Awake()
    {
        minSwipeDis = Screen.currentResolution.width / 5.0f;
        maxHeight = Screen.currentResolution.height * .6f;
    }
    void Start()
    {
        stepDistance = FindObjectOfType<LadderGenerator>().stepDistance*2;
        rHand.SetStepDistance(stepDistance,0);
        lHand.SetStepDistance(stepDistance,stepDistance/2);
        nextHandStep = lHand;
        lastHandStep = rHand;
        SwitchNextHand();

        nextHandStep.SetOutline(false);
        lastHandStep.SetOutline(false);

        windFx_Emission = windFx.emission;
    }

    // Update is called once per frame
    void Update()
    {
        if (!control) return;

        if (gameManager.buttonsControl) return;

        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y < maxHeight && Time.timeScale!=0)
        {
            touchPos = Input.mousePosition;
            tapTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.Space) && lastHandStep.check)
        {
            Tap();
        }
        if (Input.GetKeyDown(KeyCode.S) && lastHandStep.check)
        {
            Swipe();
        }

        if ((Time.time - tapTime) > .1f && tapTime!=0 && touchPos.x == Input.mousePosition.x && lastHandStep.check)
        {
            Tap();
        }

        if (Input.GetMouseButtonUp(0) )
        {
            if (lastHandStep.check && (Time.time-tapTime)<.5f)
            {
                if (Mathf.Abs(touchPos.x - Input.mousePosition.x) > minSwipeDis)
                {
                    Swipe();
                }
                else
                {
                    Tap();
                }
            }
            
            touchPos = Vector3.zero;
        }
    }

    public void Tap()
    {
        if (!control) return;
        if (nextHandStep.stopped) return;

        nextHandStep.MoveOne();
        //MoveCamPosOne();
        SwitchNextHand();
        touchPos = Vector3.zero;
        tapTime = Time.time;

        cam.SetCombo((combo.comboCounter>1));
        if (combo.comboCounter>1)
        {
            windFx_Emission.rateOverTime = Mathf.Clamp(50*(combo.comboCounter-2),50,350);
            windFx.Play();
        }

        gameManager.audioManager.PlayWhoof();
    }
    public void Swipe()
    {
        if (!control) return;
        SwitchHands();
        gameManager.audioManager.PlayWhoof();
    }
    
    public void SwitchNextHand()
    {
        if (nextHandStep==rHand)
        {
            nextHandStep = lHand;
            lastHandStep = rHand;
            cam.SetSide(-1);
        }
        else
        {
            nextHandStep = rHand;
            lastHandStep = lHand;
            cam.SetSide(1);
        }

        nextHandStep.SetOutline(true);
        lastHandStep.SetOutline(false);
        //Debug.Log("LOOOOO");
    }
    public void MoveCamPosOne()
    {
        camPos.position = camPos.position + (Vector3.up * (stepDistance/2));
    }
    void MoveCamPosMinceOne()
    {
        camPos.position = camPos.position - (Vector3.up * (stepDistance / 2));
    }

    public void HandReturnedToLastPos(bool switchNextHand = true)
    {
        if (switchNextHand) SwitchNextHand();
        MoveCamPosMinceOne();
        cam.DidntCatch();
    }

    public void SwitchHands()
    {
        if (nextHandStep == rHand)
        {
            rHand.MoveHalfOne(1);
            lHand.MoveHalfOne(-1);
        }
        else
        {
            rHand.MoveHalfOne(-1);
            lHand.MoveHalfOne(1);
        }
        SwitchNextHand();
    }

    public void Fall()
    {
        if (fell) return;
        fell = true;
        control = false;

        if (nextHandStep == rHand)
        {
            rHand.anim.SetTrigger("FallDownHand");
            lHand.anim.SetTrigger("FallTopHand");
        }
        else
        {
            lHand.anim.SetTrigger("FallDownHand");
            rHand.anim.SetTrigger("FallTopHand");
        }

        cam.Fall();
        transform.DOMoveY(Mathf.Clamp((transform.position.y - 3.5f),-2.5f,Mathf.Infinity), 3.5f).SetEase(Ease.InOutQuad);

        StartCoroutine(SlowMotionFall());
        Vibration.Vibrate(150);

        nextHandStep.SetOutline(true);
        lastHandStep.SetOutline(true);
        //combo.ResetCombo();
        gameManager.GameOver();
    }

    IEnumerator SlowMotionFall()
    {
        Time.timeScale = 0.01f;
        yield return new WaitForSecondsRealtime(.5f);
        Time.timeScale = .5f;
        yield return new WaitForSecondsRealtime(1);
        Time.timeScale = 1.0f;
    }

    public void OneStepMoved()
    {
        stepPassed++;
        //Debug.Log(stepPassed);

        if (stepPassed==ladderGenerator.nextStepForReSpawn)
        {
            ladderGenerator.GenerateLadder();
            gameManager.skybox.ChangeRandomSkybox();
        }

        if (stepPassed==ladderGenerator.stepsLength)
        {
            Won();
        }

        combo.Recharge();
        score.IncreaseScoreBy(combo.comboCounter);
        score.SetStepsTxt(stepPassed);
    }

    public void Won()
    {
        control = false;
        if (nextHandStep == rHand)
        {
            rHand.anim.SetTrigger("FinishDownHand");
            lHand.anim.SetTrigger("FinishTopHand");
        }
        else
        {
            lHand.anim.SetTrigger("FinishDownHand");
            rHand.anim.SetTrigger("FinishTopHand");
        }

        transform.DOMoveY(transform.position.y + 4, 2.0f).SetEase(Ease.OutQuad);
    }

    public int FaultGetHearts()
    {
        gameManager.canvas.LoseHeart(hearts);
        hearts = hearts - 1;
        if (hearts==0)
        {
            Fall();
        }
        return hearts;
    }

    public void CheckMaxSteps()
    {
        if (stepPassed>PlayerPrefs.GetInt("MaxSteps"))
        {
            PlayerPrefs.SetInt("MaxSteps", stepPassed);
        }
    }
}
