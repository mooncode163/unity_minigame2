using System.Collections;
using System.Collections.Generic;
using LitJson;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
public class GameLevelParse : LevelParseBase
{
    public const int ADVIDEO_LEVEL_MIN = 10;
       public List<object> listGameItems = new List<object>();
    static private GameLevelParse _main = null;
    public static GameLevelParse main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameLevelParse();
            }
            return _main;
        }
    }


    public override ItemInfo GetGuankaItemInfo(int idx)
    {
        if (listGuanka == null)
        {
            return null;
        }
        if (idx >= listGuanka.Count)
        {
            return null;
        }
        ItemInfo info = listGuanka[idx] as ItemInfo;
        return info;
    }
 
     public   ItemInfo GetItemInfo(int idx)
    {
        return listGameItems[idx] as ItemInfo;
    }
       public   ItemInfo GetLastItemInfo()
    {
        return listGameItems[listGameItems.Count-1] as ItemInfo;
    }

    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGuanka != null)
        {
            return listGuanka.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGuanka != null)
        {
            listGuanka.Clear();
        }
    }
  
    public override int ParseGuanka()
    {
        int count = 0;
        ParseGameItems();
        return count;
    }

      public string GetImagePath(string id)
    { 
        return CloudRes.main.rootPathGameRes + "/Image/"+id+".png";
    }
    public   void ParseGameItems()
    {
           if ((listGameItems != null) && (listGameItems.Count != 0))
        {
            return  ;
        }
 
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        string fileName = CloudRes.main.rootPathGameRes + "/Level/GameItems.json"; 
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;
 
        JsonData root = JsonMapper.ToObject(json);
       
        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.pic = GetImagePath(info.id);
            listGameItems.Add(info);
        }
    }

}
