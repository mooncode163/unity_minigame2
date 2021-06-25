using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LadderGenerator : MonoBehaviour
{
    public bool endless;
    public int stepsBetweenSpawns = 5;
    public int stepsLength=10;
    
    [HideInInspector] public int nextStepForReSpawn;
    public float stepDistance=1;
    public GameObject[] ladderSteps;
    public GameObject finalStep;

    public Transform sideLedder;

    bool firstGeneration = true;

    StepSc prevStep, prevprevStep;
    int lastInstantiatedStep = 0;

    [HideInInspector] public List<GameObject> ladderSteps_R, ladderSteps_L, ladderSteps_C,ladderSteps_L_OR_R;


    public Material skyboxMat;
    
    private void Awake()
    {
        ArrangeSteps();
    }

    private void Start()
    {
        if (endless)
        {
            stepsLength = stepsBetweenSpawns * 2;
        }
        CleanLadderSteps();
        GenerateLadder();

        //skyboxMat.SetColor("_top", Color.white);
    }

    public void GenerateLadder()
    {
        //Initial THREE Step
        if (firstGeneration)
        {
            Instantiate(ladderSteps[0], new Vector3(0, 0, 0), Quaternion.identity, transform);
            Instantiate(ladderSteps[0], new Vector3(0, stepDistance, 0), Quaternion.identity, transform);
            Instantiate(ladderSteps[0], new Vector3(0, stepDistance * 2, 0), Quaternion.identity, transform);

            lastInstantiatedStep = 3;

            prevStep = ladderSteps[0].GetComponent<StepSc>();
            prevprevStep = ladderSteps[0].GetComponent<StepSc>();
        }
        else
        {
            stepsLength = stepsLength + stepsBetweenSpawns;
        }
        

        
        for (lastInstantiatedStep=lastInstantiatedStep; lastInstantiatedStep < stepsLength-1; lastInstantiatedStep++)
        {
            Instantiate(GetNextStep(ref prevStep,ref prevprevStep), new Vector3(0,lastInstantiatedStep * stepDistance,0), Quaternion.identity, transform);
        }

        if (endless)
        {
            Instantiate(GetNextStep(ref prevStep, ref prevprevStep), new Vector3(0, (stepsLength - 1) * stepDistance, 0), Quaternion.identity, transform);
        }
        else
        {
            Instantiate(finalStep, new Vector3(0, (stepsLength - 1) * stepDistance, 0), Quaternion.identity, transform);
        }

        lastInstantiatedStep = stepsLength;

        nextStepForReSpawn = nextStepForReSpawn + stepsBetweenSpawns;

        SetUpSideLadder();

        firstGeneration = false;
    }

    public void CleanLadderSteps()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SetUpSideLadder()
    {
        sideLedder.localScale = new Vector3(1, (stepDistance / 2) * (stepsLength - 1), 1);
    }

    public void ArrangeSteps()
    {
        foreach (GameObject step in ladderSteps)
        {
            StepSc _step = step.GetComponent<StepSc>();
            switch (_step.stepType)
            {
                case StepSc.StepType.Long:
                    ladderSteps_C.Add(step);
                    break;
                case StepSc.StepType.Left:
                    ladderSteps_L.Add(step);
                    break;
                case StepSc.StepType.Right:
                    ladderSteps_R.Add(step);
                    break;
                case StepSc.StepType.LeftOrRight:
                    ladderSteps_L_OR_R.Add(step);
                    break;
            }
        }
    }

    public GameObject GetNextStep(ref StepSc prevStep, ref StepSc prevprevStep)
    {
        List<GameObject> newStepList = new List<GameObject>();

        if (prevStep.stepType== StepSc.StepType.Long && prevprevStep.stepType == StepSc.StepType.Long)
        {
            newStepList.AddRange(ladderSteps);
        }
        else
        {
            newStepList.AddRange(ladderSteps_C);
            newStepList.AddRange(ladderSteps_L_OR_R);

            if (prevStep.stepType != StepSc.StepType.Long)
            {
                if (prevStep.stepType!=StepSc.StepType.LeftOrRight)
                {
                    if (prevStep.stepType == StepSc.StepType.Left)
                    {
                        newStepList.AddRange(ladderSteps_R);
                    }
                    else
                    {
                        newStepList.AddRange(ladderSteps_L);
                    }
                }
                
            }
            else
            {
                if (prevprevStep.stepType != StepSc.StepType.LeftOrRight)
                {
                    if (prevprevStep.stepType == StepSc.StepType.Left)
                    {
                        newStepList.AddRange(ladderSteps_L);
                    }
                    else
                    {
                        newStepList.AddRange(ladderSteps_R);
                    }
                }
                
            }
        }
        GameObject step = newStepList[Random.Range(0, newStepList.Count)];
        prevprevStep = prevStep;
        prevStep = step.GetComponent<StepSc>();
        return step;
    }
}
