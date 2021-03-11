using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;
using UnityEngine.EventSystems;
public class GameMerge : UIView
{
    public UIBoard uIBoard;
    UIMergeItem uiItem;
    bool isTouchMove;
    bool isFirstItem;
    int indexItem =0;
 
 
    public void Awake()
    {
        base.Awake();
        LoadPrefab();
       


    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        isFirstItem = true;
        indexItem =0;
     


        LayOut();
    }


    void LoadPrefab()
    {


    }


  

    public override void LayOut()
    {
        base.LayOut();
        float x, y, w, h;
    }
 

}
