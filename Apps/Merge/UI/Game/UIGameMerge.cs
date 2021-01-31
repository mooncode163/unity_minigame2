using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine; 
using UnityEngine.UI;
using System.IO;  
public class UIGameMerge : UIGameBase//, IGameMergeDelegate
{ 
     GameMerge GameMergePrefab;
    public GameMerge GameMerge; 

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
            GameObject obj = PrefabCache.main.LoadByKey("GameMerge"); 
            if (obj != null)
            {
                GameMergePrefab = obj.GetComponent<GameMerge>();
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
         
    }
    public override void LayOut()
    {
        base.LayOut();

    }
 


}
