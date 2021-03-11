using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Moonma.IAP;
using Moonma.Share;
using UnityEngine;
using UnityEngine.UI;
using Moonma.SysImageLib;
public class UIHomeCenterBar : UIView
{

    public Button btnPlay;
    public Button btnLearn;
    public Button btnAdVideo;
    public Button btnAddLove;
    public Button btnPhoto;
    public Button btnCamera;
    public Button btnNetImage;
    void Awake()
    {


        if (btnAdVideo != null)
        {
            btnAdVideo.gameObject.SetActive(true);
            if ((Common.noad) || (!AppVersion.appCheckHasFinished))
            {
                btnAdVideo.gameObject.SetActive(false);
            }
            if (Common.isAndroid)
            {
                if (Config.main.channel == Source.GP)
                {
                    //GP市场不显示
                    btnAdVideo.gameObject.SetActive(false);
                }
            }
        }
        if (!AppVersion.appCheckHasFinished)
        {
            btnPhoto.gameObject.SetActive(false);
            btnCamera.gameObject.SetActive(false);
            btnNetImage.gameObject.SetActive(false);
        }


// btnAddLove.gameObject.SetActive(AppVersion.appCheckHasFinished);


    }
    // Use this for initialization
    void Start()
    {
        LayOut();

    }



    public override void LayOut()
    {
        base.LayOut();

        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
 

    }





    public void OnClickBtnPlay()
    {
        //AudioPlay.main.PlayAudioClip(audioClipBtn); 

        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            navi.Push(GameViewController.main);
        }
    }


    public void OnClickBtnLearn()
    {
        NaviViewController navi = this.controller.naviController;
        // navi.Push(LearnProgressViewController.main);

    }
    public void OnClickBtnAddLove()
    {
        if (this.controller != null)
        {
            NaviViewController navi = this.controller.naviController;
            // navi.Push(LoveViewController.main);
        }
    }

    public void OnClickBtnAdVideo()
    {
        AdKitCommon.main.ShowAdVideo();
    }

}
