/// <summary>
/// 点击水果下落
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generate : MonoBehaviour
{
    public const string NameDeadLine = "DeadLine";
    public const string NameBoardLine = "BoardLine";

    static public string[] imageKeyFruit = { "putao", "yintao", "juzi" ,"ningmeng","mihoutao","xihongshi","tao","boluo","yezi","xigua","daxigua"};
    static public float[] scaleFruit = { 0.1f, 0.15f, 0.2f ,0.3f,0.4f,0.45f,0.5f,0.55f,0.6f,0.65f,0.7f};
     

    private bool hasItBeenGenerated = false;//定义是否已在游戏中生成物体
    private UIMergeItem uiItem;//定义用来保存场景中未落下的水果
    private float time = 1;//计时
    float posYInit = 4.5f;

    bool isFirstRun = false;

   public bool isMouseDown = false;
   public bool isMouseUp = false;
   public bool isAutoClick = false;
    static private Generate _main = null;
    public static Generate main
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
        _main = this; 
        isFirstRun = true;
    }
 //随机获取水果
      string RandomFruitImageKey()
    {
        int rdm  = 0;
        if (imageKeyFruit.Length >= 4)//判断总水果是否大于4个
        {
              rdm = Random.Range(0, 4); 
        }
        else
        {
              rdm = Random.Range(0, imageKeyFruit.Length);
        }
        if(isFirstRun)
        {
            isFirstRun = false;
            rdm = 0;
        }

        return imageKeyFruit[rdm];
    }
 public int GetIndexOfItem(string key)
    { 
        for(int i=0;i<imageKeyFruit.Length;i++)
        {
            if(key==imageKeyFruit[i])
            {
             return i;
            }
        }
        return 0;
    }
 public string GetNextItem(string key)
    {
        string ret = "";
        for(int i=0;i<imageKeyFruit.Length;i++)
        {
            if(key==imageKeyFruit[i]&&((i+1)<imageKeyFruit.Length))
            {
                ret = imageKeyFruit[i+1];
                break;
            }
        }
        return ret;
    }

     public string GetLastItem()
    {
        string ret = "";
        if(imageKeyFruit.Length>0)
        {
             ret = imageKeyFruit[imageKeyFruit.Length-1]; 
        }
        return ret;
    }

  public UIMergeItem CreateItem(string key)
    {
        float x, y, w, h;
        UIMergeItem prefab = PrefabCache.main.LoadByKey<UIMergeItem>("UIMergeItem");
        UIMergeItem ui = (UIMergeItem)GameObject.Instantiate(prefab);
        ui.isNew = true;
        // ui.index = indexItem++; 
        // AppSceneBase.main.AddObjToMainWorld(ui.gameObject);
        ui.transform.SetParent(UIGameMerge.main.game.transform);
        ui.name = key;
        ui.spriteItem.UpdateImageByKey(key);
      
        ui.EnableGravity(false);
        float scale = scaleFruit[GetIndexOfItem(key)]*0.8f;
        ui.transform.localScale = new Vector3(scale,scale,1f);
         ui.transform.position = new Vector3(0, posYInit, 0);
        return ui;
    }

    // Update is called once per frame
    void Update()
    {

        //用作延迟生成物体
        if (time < 0.3f)
        {
            time += Time.deltaTime;
        }
        else
        {
            //判断场景中没有生成物体
            if (!hasItBeenGenerated)
            { 
                  string key = RandomFruitImageKey();
              uiItem =  CreateItem(key);
                // this.GetComponent<SizeChange>().GettingBigger(fruitInTheScene);//使物体缓慢变大

                hasItBeenGenerated = !hasItBeenGenerated;//更改hasItBeenGenerated状态
            }

            //判断是否点击
            if (Input.GetMouseButton(0))
            {
                isMouseDown = true;
            }

            if(isMouseDown)
            {
                Debug.Log("autoclick MouseClickDown isMouseDown ");
                isMouseDown = false;
                //// float mousePosition_x = Input.mousePosition.x;//获取点击位置(只需要x轴位置)//这样获取的不是世界坐标系 所以废弃

                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);//获取点击位置
    
               if(isAutoClick)
               {
                   mousePosition.x =0;
               }
                if(uiItem!=null)
                {
                uiItem.transform.position = new Vector3(mousePosition.x, posYInit, 0);//更改水果在场景中的位置
                }
            }

            //判断是否完成点击
            if (Input.GetMouseButtonUp(0))
              {
                isMouseUp = true;
            }

            if(isMouseUp)
            {
                  Debug.Log("autoclick MouseClickUp isMouseUp ");
                isMouseUp = false;
                //让水果获得重力下降
                // fruitInTheScene.GetComponent<Rigidbody2D>().simulated = true;
                if(uiItem!=null)
                {
                uiItem.EnableGravity(true); 
                uiItem.isNew = false;
                uiItem = null;//清除保存的水果
                 }
                hasItBeenGenerated = !hasItBeenGenerated;//更改hasItBeenGenerated状态

                time = 0;

            }
        }


    }

}
