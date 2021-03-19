using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;
using UnityEngine.EventSystems;
public class GameMerge : UIView
{
    public const string NameDeadLine = "DeadLine";
    public const string NameBoardLine = "BoardLine";
    public UIBoard uIBoard;
    public UISprite uiProp;
    bool isTouchMove;
    bool isFirstItem;
    int indexItem = 0;


    public List<object> listItem = new List<object>();

    // static public string[] imageKeyFruit = { "putao", "yintao", "juzi" ,"ningmeng","mihoutao","xihongshi","tao","boluo","yezi","xigua","daxigua"};
    // static public float[] scaleFruit = { 0.1f, 0.15f, 0.2f ,0.3f,0.4f,0.45f,0.5f,0.55f,0.6f,0.65f,0.7f};
    public const float ScaleStart = 0.1f;

    private bool hasItBeenGenerated = false;//定义是否已在游戏中生成物体
    private UIMergeItem uiItem;//定义用来保存场景中未落下的水果
    private float time = 1;//计时
    float posYInit = 4.5f;

    bool isFirstRun = false;

    public bool isMouseDown = false;
    public bool isMouseUp = false;
    public bool isAutoClick = false;

    public static GameMerge main;
    public void Awake()
    {
        main = this;
        base.Awake();
        LoadPrefab();
        BoxCollider2D box = this.gameObject.GetComponent<BoxCollider2D>();
        box.size = Common.GetWorldSize(mainCam);

        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;
        uiProp.SetActive(false);
        isFirstRun = true;
        Clear();

    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
        isFirstItem = true;
        indexItem = 0;



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

   public void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    Debug.Log("GameMerge down id=" + this.id);
                    float z = uiProp.transform.position.z;
                    Vector3 pos = eventData.pointerCurrentRaycast.worldPosition;
                    pos.z = z;
                    // uiProp.transform.position=pos;
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    Debug.Log("GameMerge up id=" + this.id);
                }
                break;
            case UITouchEvent.STATUS_Click:
                {

                }
                break;

        }

        UpdateEvent(status);
    }

    public void UpdateProp(string keypic)
    {
        uiProp.UpdateImageByKey(keypic);
    }
    public void ShowProp(bool isShow)
    {
        uiProp.SetActive(isShow);
        if (isShow)
        {
            float z = uiProp.transform.position.z;
            Vector3 pos = Vector3.zero;
            pos.z = z;
            uiProp.transform.position = pos;
        }
    }



    void Clear()
    {
        foreach (object obj in listItem)
        {
            UIMergeItem ui = obj as UIMergeItem;
            DestroyImmediate(ui.gameObject);
        }
        listItem.Clear();
    }
    int GetTotalItems()
    {
        return GameLevelParse.main.listGameItems.Count;
    }

    // 判断场景里是否有掉落下来的球
    public bool IsHasFalledBall()
    {
        return listItem.Count > 1 ? true : false;
    }

    string GetItemId(int idx)
    {
        ItemInfo info = GameLevelParse.main.GetItemInfo(idx);
        return info.id;
    }
    //随机获取水果
    string RandomFruitImageKey()
    {
        int rdm = 0;
        if (GetTotalItems() >= 4)//判断总水果是否大于4个
        {
            rdm = Random.Range(0, 4);
        }
        else
        {
            rdm = Random.Range(0, GetTotalItems());
        }
        if (isFirstRun)
        {
            isFirstRun = false;
            rdm = 0;
        }

        return GetItemId(rdm);
    }
    public int GetIndexOfItem(string key)
    {
        for (int i = 0; i < GetTotalItems(); i++)
        {
            if (key == GetItemId(i))
            {
                return i;
            }
        }
        return 0;
    }
    public string GetNextItem(string key)
    {
        string ret = "";
        for (int i = 0; i < GetTotalItems(); i++)
        {
            if (key == GetItemId(i) && ((i + 1) < GetTotalItems()))
            {
                ret = GetItemId(i + 1);
                break;
            }
        }
        return ret;
    }

    public string GetLastItem()
    {
        string ret = "";
        if (GetTotalItems() > 0)
        {
            ret = GetItemId(GetTotalItems() - 1);
        }
        return ret;
    }

    void OnRestPlay()
    {
        //  Invoke("OnRestPlayInternal",0.2f);
        OnRestPlayInternal();
    }

    void OnRestPlayInternal()
    {
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Play;
        UIGameMerge.main.game.ShowProp(false);
    }
    // 改变类型为toId
    public void ChangeItem(string toId)
    {
        if (uiItem != null)
        {
            uiItem.id = toId;
            uiItem.name = toId;
            string pic = GameLevelParse.main.GetImagePath(toId);
            uiItem.spriteItem.UpdateImage(pic);
        }

        OnRestPlay();
    }
    public void DeleteItem(UIMergeItem ui)
    {
        foreach (object obj in listItem)
        {
            UIMergeItem uilist = obj as UIMergeItem;
            if (uilist == ui)
            {
                DestroyImmediate(ui.gameObject);
                listItem.Remove(obj);
                break;
            }
        }
        OnRestPlay();
    }

    public void RemoveItemFromList(GameObject objitem)
    {
        foreach (object obj in listItem)
        {
            UIMergeItem uilist = obj as UIMergeItem;
            UIMergeItem item = objitem.GetComponent<UIMergeItem>();
            if (uilist == item)
            {
                listItem.Remove(obj);
                break;
            }
        }
    }

    // 摧毁所有的同类
    public void DeleteAllItemsOfId(string id)
    {
        foreach (object obj in listItem)
        {
            UIMergeItem ui = obj as UIMergeItem;
            if (ui.id == id)
            {
                DestroyImmediate(ui.gameObject);
                // listItem.Remove(obj);
            }
        }

        for (int i = 0; i < listItem.Count; i++)
        {
            UIMergeItem ui = listItem[i] as UIMergeItem;
            if (ui.id == id)
            {
                listItem.Remove(ui);
            }
        }
        OnRestPlay();
    }
    public UIMergeItem CreateItem(string key)
    {
        float x, y, w, h;
        UIMergeItem prefab = PrefabCache.main.LoadByKey<UIMergeItem>("UIMergeItem");
        UIMergeItem ui = (UIMergeItem)GameObject.Instantiate(prefab);
        ui.isNew = true;
        ui.id = key;
        // ui.index = indexItem++; 
        // AppSceneBase.main.AddObjToMainWorld(ui.gameObject);
        ui.transform.SetParent(UIGameMerge.main.game.transform);
        ui.name = key;
        string pic = GameLevelParse.main.GetImagePath(key);
        ui.spriteItem.UpdateImage(pic);

        ui.EnableGravity(false);
        float scale = (ScaleStart + 0.05f * GetIndexOfItem(key)) * 0.8f;
        ui.transform.localScale = new Vector3(scale, scale, 1f);
        ui.transform.localPosition = new Vector3(0, posYInit, -1f);
        listItem.Add(ui);
        return ui;
    }

    void Update()
    {
        //用作延迟生成物体
        if (time < 0.2f)
        {
            time += Time.deltaTime;
        }
        else
        {
            //判断场景中没有生成物体
            if (!hasItBeenGenerated)
            // if (isMouseDown)
            {
                string key = RandomFruitImageKey();
                uiItem = CreateItem(key);
                // this.GetComponent<SizeChange>().GettingBigger(fruitInTheScene);//使物体缓慢变大

                hasItBeenGenerated = true;//更改hasItBeenGenerated状态
            }

            if (isAutoClick)
            {
                UpdateEvent(UITouchEvent.STATUS_TOUCH_DOWN);
            }

        }


    }
    public void UpdateEvent(int status)
    {

        if (UIGameMerge.main.gameStatus == UIGameMerge.Status.Prop)
        {
            return;
        }

        {


            //判断是否点击
            // if (Input.GetMouseButton(0))
            if (UITouchEvent.STATUS_TOUCH_DOWN == status)
            {
                isMouseDown = true;
            }



            if (isMouseDown && (UIGameMerge.main.gameStatus == UIGameMerge.Status.Play))
            {
                // string key = RandomFruitImageKey();
                // uiItem = CreateItem(key);

                // Debug.Log("autoclick MouseClickDown isMouseDown ");
                isMouseDown = false;
                //// float mousePosition_x = Input.mousePosition.x;//获取点击位置(只需要x轴位置)//这样获取的不是世界坐标系 所以废弃

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取点击位置

                if (isAutoClick)
                {
                    mousePosition.x = 0;
                }
                if (uiItem != null)
                {
                    if (uiItem.isNew)
                    {
                        Vector3 pos = new Vector3(mousePosition.x, posYInit, 0);//更改水果在场景中的位置

                        float value = 3f;
                        float ratio = 0.2f;
                        if (isAutoClick)
                        {
                            ratio = 1f;
                        }
                        // 生成物体 使用随机防止同地点击无限堆高
                        // uiItem.transform.position = pos + new Vector3(UnityEngine.Random.Range(-value, value) * ratio, UnityEngine.Random.Range(-value, value) * ratio, 0);//!
                        uiItem.transform.position = pos + new Vector3(UnityEngine.Random.Range(-value, value) * ratio,  0, 0);//!


                    }
                }
            }

            if ((UITouchEvent.STATUS_TOUCH_MOVE == status)&&(!isAutoClick))
           {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取点击位置
                if (uiItem != null)
                {
                    if (uiItem.isNew)
                    {
                        Vector3 pos = new Vector3(mousePosition.x, posYInit, 0);//更改水果在场景中的位置
                       uiItem.transform.position = pos;//! 
                    }
                }
            }
            //判断是否完成点击
            // if (Input.GetMouseButtonUp(0))
            if (UITouchEvent.STATUS_TOUCH_UP == status)
            {
                isMouseUp = true;
            }

            if (isMouseUp && (UIGameMerge.main.gameStatus == UIGameMerge.Status.Play))
            {
                // Debug.Log("autoclick MouseClickUp isMouseUp ");
                isMouseUp = false;
                //让水果获得重力下降
                // fruitInTheScene.GetComponent<Rigidbody2D>().simulated = true;
                if (uiItem != null)
                {
                    uiItem.EnableGravity(true);
                    uiItem.isNew = false;
                    // uiItem = null;//清除保存的水果
                }
                hasItBeenGenerated = false;//更改hasItBeenGenerated状态

                time = 0;

            }
        }


    }
}
