using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class ComboSc : MonoBehaviour
{
    public Image bar;
    Sequence mySequence;
    [HideInInspector] public int comboCounter = 1;

    float comboTime=.9f;

    public TextMeshProUGUI comboTxt;
    public TextMeshProUGUI comboPopupTxt;

    public Animator anim;

    public GameManager gameManager;
    //public ParticleSystem windFx;
    [HideInInspector] public int topCombo=0;

    private void Awake()
    {
        mySequence = DOTween.Sequence();
        ResetCombo();
    }

    
    public void Recharge()
    {
        StopCoroutine("waitToResetCombo");
        if (mySequence.IsPlaying())
        {
            mySequence.Kill();
            UpgradeCombo();
        }
        else
        {
            comboCounter = 1;
        }
        
        mySequence = DOTween.Sequence();
        mySequence.Append(bar.transform.DOScaleX(1, .1f)).Append(bar.transform.DOScaleX(0, comboTime)).SetEase(Ease.Linear);

        if (gameObject.activeSelf) StartCoroutine("waitToResetCombo");
    }

    IEnumerator waitToResetCombo()
    {
        yield return new WaitForSeconds(comboTime);
        ResetCombo();
    }
    public void ResetCombo()
    {
        CheckHighCombo();
        /*if (comboCounter>3)
        {
            gameManager.audioManager.PlayComboLost();
        }*/
        comboCounter = 1;
        comboTxt.text = "";
        comboPopupTxt.text = comboTxt.text;
        //gameObject.SetActive(false);
        anim.SetBool("Combo", false);

        gameManager.cam.SetCombo(false);
        gameManager.skybox.SetHeight(5*comboCounter);

    }
    public void UpgradeCombo()
    {
        if (comboCounter==1)
        {
            gameObject.SetActive(true);
            anim.SetBool("Combo", true);
        }
        anim.SetBool("ComboXPopup", true);
        comboCounter++;
        comboTxt.text = "x" + comboCounter;
        comboPopupTxt.text = comboTxt.text;

        gameManager.skybox.SetHeight(comboCounter);
        //comboTxt.transform.DOComplete();
        //comboTxt.transform.DOPunchScale(Vector3.one *2.5f, .7f, 1, 1);
    }

    public void CheckHighCombo()
    {
        if (comboCounter>PlayerPrefs.GetInt("HighCombo"))
        {
            PlayerPrefs.SetInt("HighCombo", comboCounter);
        }

        if (topCombo< comboCounter)
        {
            topCombo = comboCounter;
        }
    }

}