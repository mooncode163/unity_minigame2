using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

/*
ui 界面参考
https://play.google.com/store/apps/details?id=com.hz.Android153.FruitSliceMaster

*/
public class UIGameMerge : UIGameBase//, IGameMergeDelegate
{
    GameMerge GameMergePrefab;
    public GameMerge game;
    public UIText titleScore;
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
        autoIndex =0;


    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        
        AutoClick();

        if(GameManager.main.isLoadGameScreenShot)
        {
            if(LevelManager.main.gameLevel>1)
            {
                
            // OnGameFinish(false);
            }
        }
        OnUIDidFinish(autoClickTime*autoClickCount*1.2f);

       
    }

       IEnumerator MouseClickUp(float time,int idx)
    {
         yield return new WaitForSeconds(time); 
         Generate.main.isMouseUp = true;
         Debug.Log("autoclick MouseClickUp idx ="+idx);
         
    }

         IEnumerator MouseClickDown(float time,int idx)
    {
         yield return new WaitForSeconds(time); 
         Generate.main.isMouseDown = true;
         Debug.Log("autoclick MouseClickDown idx ="+idx);
    }

    public void AutoClick()
    {
        if(!GameManager.main.isLoadGameScreenShot)
        {
           return;
        }
        Generate.main.isAutoClick = true;
        int count = autoClickCount; 
        float time = autoClickTime;
        //for(int i=0;i<count;i++)
        { 
            StartCoroutine(MouseClickDown(time,autoIndex));
            StartCoroutine(MouseClickUp(time,autoIndex));
        }
        autoIndex++;
        if(autoIndex<count)
        {
        Invoke("AutoClick",time*2);
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

        GameMerge prefab = PrefabCache.main.LoadByKey<GameMerge>("GameMerge");
         game = (GameMerge)GameObject.Instantiate(prefab);
        
        AppSceneBase.main.AddObjToMainWorld(game.gameObject);
        UIViewController.ClonePrefabRectTransform(prefab.gameObject,game.gameObject);
        GameData.main.score = 0;
        UpdateScore();
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
        titleScore.text = Language.main.GetString("Score")+":"+GameData.main.score.ToString();
    }

public void OnGameFinish(  bool isFail)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(LevelManager.main.placeLevel);
        string key = "UIGameWin";
          string strPrefab ="";
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


}
