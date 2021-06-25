using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Transform clouds;
    public LadderGenerator ladderGenerator;
    public CameraController cam;
    public UIFunctions canvas;
    public PlayerMovement plMovement;
    public SkyboxController skybox;
    public ScoreSc score;
    public ComboSc combo;
    public ShopSystem shop;

    public bool buttonsControl = false;

    [HideInInspector] public bool started = false;

    public AudioManager audioManager;

    private void Awake()
    {
        Time.timeScale = 1;
        if (!PlayerPrefsExtra.GetBool("FirstTimeDone"))
        {
            PlayerPrefsExtra.SetBool("FirstTimeDone", true);
            PlayerPrefsExtra.SetBool("Vibrate", true);
            PlayerPrefsExtra.SetBool("Sound", true);
            PlayerPrefsExtra.SetBool("Buttons", true);
        }
    }
    private void Start()
    {
        canvas.SetButtonsControlState(PlayerPrefsExtra.GetBool("Buttons"));
        if (!ladderGenerator.endless)
        {
            clouds.transform.position = new Vector3(clouds.transform.position.x, clouds.transform.position.y + ladderGenerator.stepsLength * ladderGenerator.stepDistance, clouds.transform.position.z);
            clouds.transform.localScale = Vector3.one * (ladderGenerator.stepsLength / 11.0f);
        }

    }

    public void GameStart()
    {
        started = true;
        cam.CamStart();
        plMovement.control = true;

        if (!PlayerPrefsExtra.GetBool("Tuto") && !PlayerPrefsExtra.GetBool("Buttons"))
        {
            canvas.TutoFunc();
        }
    }

    public void GameOver()
    {
        combo.CheckHighCombo();
        score.CheckHighScore();
        plMovement.CheckMaxSteps();
        canvas.GameOver();
        audioManager.PlayLose();
    }

    public void Shoping(bool isShoping)
    {
        cam.Shoping(isShoping);
        if (isShoping)
        {
            shop.DisplayShopPanel();
        }
    }
}
