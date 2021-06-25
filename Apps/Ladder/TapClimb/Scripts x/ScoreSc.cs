using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreSc : MonoBehaviour
{
    public TextMeshProUGUI scoreTxt;
    public int score;
    float currentDisplayedScore;
    public float increaseTime = 1;
    public Animator anim;

    public TextMeshProUGUI stepsTxt;

    private void Start()
    {
        scoreTxt.text = "";
        stepsTxt.text = "";
    }

    public void IncreaseScoreBy(int increament)
    {
        score = score + increament;
        UpdateScoreTxt();
    }

    public void UpdateScoreTxt()
    {
        StopCoroutine("increaseScoreOvertime");

        anim.SetBool("ScoreIncreasing", true);
        StartCoroutine("increaseScoreOvertime");
    }

    IEnumerator increaseScoreOvertime()
    {
        float deltaScore = score - currentDisplayedScore;
        float increament = deltaScore / (increaseTime / Time.deltaTime);

        increament = Mathf.Clamp(increament, 1.0f, Mathf.Infinity);

        while (currentDisplayedScore <score)
        {
            currentDisplayedScore = currentDisplayedScore + increament;
            scoreTxt.text = currentDisplayedScore.ToString("00");
            yield return new WaitForEndOfFrame();
        }
        currentDisplayedScore = score;
        scoreTxt.text = currentDisplayedScore.ToString("00");
        anim.SetBool("ScoreIncreasing", false);
    }

    public void SetStepsTxt(int steps)
    {
        stepsTxt.text = "Steps: " + steps;
    }

    public void CheckHighScore()
    {
        if (score>PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }
}
