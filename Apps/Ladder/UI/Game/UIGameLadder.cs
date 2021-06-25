using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class UIGameLadder : UIGameBase//, IGameLadderDelegate
{
    GameLadder gamePrefab;
    public GameLadder game;

    static private UIGameLadder _main = null;
    public static UIGameLadder main
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
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        LayOut();
        UpdateGuankaLevel(LevelManager.main.gameLevel);
        Invoke("AutoReady", 1f);
    }

    public void AutoReady()
    {

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
            GameObject obj = PrefabCache.main.LoadByKey("GameLadder");
            if (obj != null)
            {
                gamePrefab = obj.GetComponent<GameLadder>();
            }

        }





    }

    void InitUI()
    {


        OnUIDidFinish();
    }


    public override void UpdateGuankaLevel(int level)
    {
        base.UpdateGuankaLevel(level);
        {
            game = (GameLadder)GameObject.Instantiate(gamePrefab);
            AppSceneBase.main.AddObjToMainWorld(game.gameObject);
            game.transform.localPosition = new Vector3(0f, 0f, -1f);
            UIViewController.ClonePrefabRectTransform(gamePrefab.gameObject, game.gameObject);
        }


    }
    public override void LayOut()
    {
        base.LayOut();

    }



}
