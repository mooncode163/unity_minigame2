using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class UIFunctions : MonoBehaviour
{
    public GameManager gameManager;
    public Animator anim;
    public GameObject tapSpace;

    public Image heart2, heart1;
    public Color heartBreakColor;

    public TextMeshProUGUI topComboTxt;

    public TextMeshProUGUI highScore, highCombo, maxSteps;

    bool isShoping = false;

    public Transform crownScore, crownCombo, crownSteps;

    public GameObject panelControlButtons;

    public Image soundBtn, vibrateBtn;
    public Sprite soundImg, muteImg, vibrateImg, noVibrateImg;


    private void Start()
    {
        highScore.text = PlayerPrefs.GetInt("HighScore").ToString();
        highCombo.text = "x"+ PlayerPrefs.GetInt("HighCombo").ToString();
        maxSteps.text = PlayerPrefs.GetInt("MaxSteps").ToString();

        SoundFunc(); SoundFunc();
        VibrationFunc(); VibrationFunc();
    }
    public void Replay()
    {
        Time.timeScale = 1.0f;
        anim.SetTrigger("FadeOut");

        //50% showing ads
        if (Random.Range(0,2)==0)
        {
            try{
            
                FindObjectOfType<ADManager>().Display_InterstitialAd();
            
            } catch (System.Exception){}
            
        }

        StartCoroutine(waitToReloadScene());
    }
    IEnumerator waitToReloadScene()
    {
        yield return new WaitForSeconds(.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void TapStart()
    {
        if (isShoping) return;

        gameManager.GameStart();
        anim.SetTrigger("Start");
        tapSpace.SetActive(false);
        anim.SetBool("DisplayingControl", false);

        if (gameManager.buttonsControl)
        {
            gameManager.plMovement.Tap();
        }
    }

    public void PauseFunc()
    {
        anim.SetBool("Pause", !anim.GetBool("Pause"));
        if (anim.GetBool("Pause"))
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void GameOver()
    {
        topComboTxt.text = "Top Combo x" + gameManager.combo.topCombo;
        anim.SetTrigger("GameOver");
    }

    public void LoseHeart(int num)
    {
        anim.SetTrigger("SliceHeart");
        heart1.transform.parent.DOPunchScale(Vector3.one * .6f, .6f, 10, 0);
        Image heart = null;
        switch (num)
        {
            case 2:
                heart = heart2;
                break;
            case 1:
                heart = heart1;
                break;
        }

        if (heart!=null)
        {
            heart.color = heartBreakColor;
        }
    }

    public void Shopfunc()
    {
        isShoping = !isShoping;
        gameManager.Shoping(isShoping);
        anim.SetBool("Shoping",isShoping);
    }

    public void CheckIfHighScores()
    {
        if (gameManager.score.score >= PlayerPrefs.GetInt("HighScore"))
        {
            crownScore.DOScale(Vector3.one, .2f);
        }

        if (gameManager.combo.topCombo >= PlayerPrefs.GetInt("HighCombo"))
        {
            crownCombo.DOScale(Vector3.one, .2f);
        }

        if (gameManager.plMovement.stepPassed >= PlayerPrefs.GetInt("MaxSteps"))
        {
            crownSteps.DOScale(Vector3.one, .2f);
        }
    }

    public void ControlFunc()
    {
        anim.SetBool("DisplayingControl", !anim.GetBool("DisplayingControl"));
    }
    public void ControlButtons()
    {
        anim.SetBool("Buttons", true);
        SetButtonsControlState(true);
    }
    public void ControlGestures()
    {
        anim.SetBool("Buttons", false);
        SetButtonsControlState(false);
    }

    public void SetButtonsControlState(bool state)
    {
        anim.SetBool("Buttons", state);
        gameManager.buttonsControl = state;
        PlayerPrefsExtra.SetBool("Buttons", state);
        panelControlButtons.SetActive(state);
    }

    public void TutoFunc()
    {
        int maxTuto = 2;
        
        gameManager.plMovement.control = gameManager.started;

        if (anim.GetInteger("Tuto") == 1) gameManager.plMovement.Tap();
        if (anim.GetInteger("Tuto") == 2) gameManager.plMovement.Swipe();

        gameManager.plMovement.control = false;

        if (anim.GetInteger("Tuto") != maxTuto)
        {
            anim.SetInteger("Tuto", anim.GetInteger("Tuto") + 1);
        }
        else
        {
            anim.SetInteger("Tuto", 0);
            PlayerPrefsExtra.SetBool("Tuto", true);
            gameManager.plMovement.control = gameManager.started;
        }
    }

    public void ClickSound()
    {
        gameManager.audioManager.PlayClick1();
    }

    public void PopSound()
    {
        gameManager.audioManager.PlayClick2();
    }

    public void SoundFunc()
    {
        if (!PlayerPrefsExtra.GetBool("Sound"))
        {
            soundBtn.sprite = soundImg;
            PlayerPrefsExtra.SetBool("Sound", true);
        }
        else
        {
            soundBtn.sprite = muteImg;
            PlayerPrefsExtra.SetBool("Sound", false);
        }

        gameManager.audioManager.SetSoundState(PlayerPrefsExtra.GetBool("Sound"));
    }
    public void VibrationFunc()
    {
        if (!PlayerPrefsExtra.GetBool("Vibrate"))
        {
            PlayerPrefsExtra.SetBool("Vibrate", true);
            vibrateBtn.sprite = vibrateImg;
        }
        else
        {
            PlayerPrefsExtra.SetBool("Vibrate", false);
            vibrateBtn.sprite = noVibrateImg;
        }
        Vibration.SetVibrationState(PlayerPrefsExtra.GetBool("Vibrate"));
        //Vibration.Vibrate(40);
    }
}
