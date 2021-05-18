
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UIOptionGame : UIViewPop
{
    public UIImage imageBg;
    public UIImage imageItem;

    public UIProgress progressSpeed;
    public UIProgress progressBounce;
    public UIProgress progressRotation;

    public UIText textSpeed;
    public UIText textBounce;
    public UIText textRotation;
    public UIButton btnSave;

    float speed;
    float bounce;
    float rotation;
    public void Awake()
    {
        base.Awake();

        progressSpeed.callBackProgress = OnUIProgress;
        progressBounce.callBackProgress = OnUIProgress;
        progressRotation.callBackProgress = OnUIProgress;

        progressSpeed.percent = (GameData.main.speed * 100.0f / GameData.MaxSpeed);
        progressBounce.percent = (GameData.main.bounce * 100.0f / GameData.MaxBounce);
        progressRotation.percent = (GameData.main.rotation * 100.0f / GameData.MaxRotation);
        this.LayOut();
    }
    public void Start()
    {
        base.Start();
        this.LayOut();
    }
    public override void LayOut()
    {
        base.LayOut();
    }

    public void OnClickBtnBack()
    {
        Close();
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Play;
    }
    public void OnClickBtnSave()
    {

        GameData.main.speed = speed;
        GameData.main.bounce = bounce;
        GameData.main.rotation = rotation;

        OnClickBtnBack();
    }

    public void OnUIProgress(UIProgress ui)
    {
        if (ui == progressSpeed)
        {
            speed = (ui.percent * GameData.MaxSpeed / 100.0f);
        }
        if (ui == progressBounce)
        {
            bounce = (ui.percent * GameData.MaxBounce / 100.0f);
        }
        if (ui == progressRotation)
        {
            rotation = (ui.percent * GameData.MaxRotation / 100.0f);
        }
    }
}

