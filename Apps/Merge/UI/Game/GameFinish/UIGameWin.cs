using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UIGameWin : UIViewPop
{
    public UIImage imageBg;
    public UIImage imageLogo;
    public UIImage imageItem0;
    public UIImage imageItem1;
    public UIImage imageItem2;
    public UIText textTitle;
    public Button btnRestart; //从第一关开始

    public void Awake()
    {
        base.Awake();
        List<UIImage> listItem = new List<UIImage>();
        listItem.Add(imageItem0);
        listItem.Add(imageItem1);
        listItem.Add(imageItem2);

        for (int i = 0; i < listItem.Count; i++)
        {
            ItemInfo info = GameLevelParse.main.GetItemInfo(i);
            string pic = GameLevelParse.main.GetImagePath(info.id);
            UIImage ui = listItem[i];
            ui.index = i;
            ui.id = info.id;
            ui.UpdateImage(pic);
        }

        {
            ItemInfo info = GameLevelParse.main.GetLastItemInfo();
            string pic = GameLevelParse.main.GetImagePath(info.id);
            imageLogo.UpdateImage(pic);
        }

        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.7f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        base.LayOut();
    }

    public void OnClickBtnRestart()
    {
        Close();

        // placeLevel 不改变
        // LevelManager.main.placeLevel = 0;
        LevelManager.main.gameLevel = 0;
        LevelManager.main.gameLevelFinish = -1;
        GameManager.main.GotoPlayAgain();
    }

}
