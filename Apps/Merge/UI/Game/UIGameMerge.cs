using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Threading;

/*
ui 界面参考
https://play.google.com/store/apps/details?id=com.hz.Android153.FruitSliceMaster

*/
public class UIGameMerge : UIGameBase//, IGameMergeDelegate
{
    public enum Status
    {
        Play,
        Prop,
    }
    GameMerge GameMergePrefab;
    public GameMerge game;
    public UIToolBar uiToolBar;
    public UIText titleScore;

    public Status gameStatus;
    public UIPopProp.Type typeProp;
    int autoIndex = 0;
    float autoClickTime = 0.2f;
    static public int autoClickCount = 200;
    static private UIGameMerge _main = null;
    public static UIGameMerge main
    {
        get
        {
            if (_main == null)
            {

            }
            return _main;
        }

    }
    public void Awake()
    {
        base.Awake();
        LoadPrefab();
        _main = this;
        autoIndex = 0;
        gameStatus = Status.Play;

        if (AppVersion.appCheckHasFinished)
        {
            UIGameAppCenter ui = ShowGameAppCenter();
            LayOutRelation ly = ui.gameObject.AddComponent<LayOutRelation>();
            ly.align = LayOutRelation.Align.UP_LEFT;
            ly.offset = new Vector2(0, 460f);
        }

        LayOut();

    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
        UpdateGuankaLevel(LevelManager.main.gameLevel);

        AutoClick();
        bool isShowProp = false;
        if (GameManager.main.isLoadGameScreenShot)
        {
            if (LevelManager.main.gameLevel > 1)
            {
                isShowProp = true;
                //   OnGameFinish(false);
                Invoke("ShowProp", autoClickTime * autoClickCount / 3);
            }
        }
        if (!isShowProp)
        {

            OnUIDidFinish(autoClickTime * autoClickCount * 1.2f);

        }
        // OnGameFinish(true);
    }

    public void ShowProp()
    {
        uiToolBar.OnClickBtnBomb();
        OnUIDidFinish();
    }

    IEnumerator MouseClickUp(float time, int idx)
    {
        yield return new WaitForSeconds(time);
        // Thread.Sleep((int)(time*1000));
        GameMerge.main.isMouseUp = true;
        Debug.Log("autoclick MouseClickUp idx =" + idx);

    }

    IEnumerator MouseClickDown(float time, int idx)
    {
        yield return new WaitForSeconds(time);
        // Thread.Sleep((int)(time*1000));
        GameMerge.main.isMouseDown = true;
        Debug.Log("autoclick MouseClickDown idx =" + idx);
    }

    public void AutoClick()
    {
        if (!GameManager.main.isLoadGameScreenShot)
        {
            return;
        }
        GameMerge.main.isAutoClick = true;
        //for(int i=0;i<count;i++)
        {
            StartCoroutine(MouseClickDown(autoClickTime, autoIndex));
            StartCoroutine(MouseClickUp(autoClickTime, autoIndex));
        }
        autoIndex++;
        if (autoIndex < autoClickCount)
        {
            Invoke("AutoClick", autoClickTime * 2);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            OnClickBtnBack();
        }

    }

    void LoadPrefab()
    {

        {
            GameObject obj = PrefabCache.main.LoadByKey("GameMerge");
            if (obj != null)
            {
                GameMergePrefab = obj.GetComponent<GameMerge>();
            }

        }


    }



    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        GameLevelParse.main.CleanGuankaList();
        GameLevelParse.main.ParseGuanka();
        GameMerge prefab = PrefabCache.main.LoadByKey<GameMerge>("GameMerge");
        game = (GameMerge)GameObject.Instantiate(prefab);

        AppSceneBase.main.AddObjToMainWorld(game.gameObject);
        UIViewController.ClonePrefabRectTransform(prefab.gameObject, game.gameObject);
        GameData.main.score = 0;
        UpdateScore();
        if (GameData.main.IsCustom() && (!GameData.main.HasCustomImage))
        {
            uiToolBar.ShowImageSelect(false);
        }
    }
    public override void LayOut()
    {
        base.LayOut();

    }



    /// <summary>
    /// 显示分数
    /// </summary>
    public void UpdateScore()
    {
        titleScore.text = Language.main.GetString("Score") + ":" + GameData.main.score.ToString();
        LayOut();
    }

    public void OnGameFinish(bool isFail)
    {
        if (GameManager.main.isLoadGameScreenShot)
        {
            return;
        }
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string key = "UIGameWin";
        string strPrefab = "";
        //show game win
        if (isFail)
        {
            ShowAdInsert(GAME_AD_INSERT_SHOW_STEP, false);
            AudioPlay.main.PlayFile(AppRes.AUDIO_Fail);
            key = "UIGameFail";
            strPrefab = ConfigPrefab.main.GetPrefab(key);
            PopUpManager.main.Show<UIGameFail>(strPrefab, popup =>
            {
                Debug.Log("UIGameFail Open ");
                // popup.UpdateItem(info);

            }, popup =>
            {


            });

        }
        else
        {
            AudioPlay.main.PlayFile(AppRes.AUDIO_Win);
            Debug.Log("  OnGameWin");
            LevelManager.main.gameLevelFinish = LevelManager.main.gameLevel;
            // OnGameWinBase();

            strPrefab = ConfigPrefab.main.GetPrefab(key);
            PopUpManager.main.Show<UIGameWin>(strPrefab, popup =>
         {
             Debug.Log("UIGameWin Open ");
             // popup.UpdateItem(info);

         }, popup =>
         {


         });
        }



    }

    public void OnGameProp(UIPopProp ui, UIPopProp.Type type)
    {
        typeProp = type;

        Debug.Log("OnGameProp typeProp=" + typeProp);
        switch (type)
        {
            case UIPopProp.Type.Hammer:
                {

                }
                break;
            case UIPopProp.Type.Magic:
                {
                    GameMerge.main.ChangeItem(ui.idChangeTo);
                }
                break;
            case UIPopProp.Type.Bomb:
                {

                }
                break;
        }
    }

}
