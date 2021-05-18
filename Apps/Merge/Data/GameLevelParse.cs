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
    public List<object> listGameItemDefault = new List<object>();
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
        if (listGameItems == null)
        {
            return null;
        }
        if (idx >= listGameItems.Count)
        {
            return null;
        }
        ItemInfo info = listGameItems[idx] as ItemInfo;
        return info;
    }

    public ItemInfo GetItemInfo(int idx)
    {
        return listGameItems[idx] as ItemInfo;
    }
    public ItemInfo GetLastItemInfo()
    {
        return listGameItems[listGameItems.Count - 1] as ItemInfo;
    }

    public override int GetGuankaTotal()
    {
        ParseGuanka();
        if (listGameItems != null)
        {
            return listGameItems.Count;
        }
        return 0;
    }

    public override void CleanGuankaList()
    {
        if (listGameItems != null)
        {
            listGameItems.Clear();
        }
    }

    public override int ParseGuanka()
    {
        int count = 0;
        ParseGameItems();
        ParseGameItemsDefault();
        return count;
    }
    public string GetCustomImagePath(string id)
    {
        string pic = GetSaveCustomImagePath(id);
        if (!FileUtil.FileIsExist(pic))
        {
            pic = GetImagePathPlace(id, 0);
        }
        return pic;
    }

    public string GetSaveCustomImagePath(string id)
    {
        return GameData.main.CustomImageRootDir + "/" + id + ".png";
    }

    public bool IsRenderCustomImage(string id)
    {
        if (GameData.main.IsCustom())
        { 
            if (IsHasCustomImage(id))
            {
                return true;
            }
        }
        return false;
    }

    public bool IsHasCustomImage(string id)
    {
        string pic = GetSaveCustomImagePath(id);
        if (FileUtil.FileIsExist(pic))
        {
            return true;
        }
        return false;
    }

    public string GetImagePath(string id)
    {
        if (GameData.main.IsCustom())
        {
            return GetCustomImagePath(id);
        }
        int idx = LevelManager.main.placeLevel;
        return GetImagePathPlace(id, idx);

    }

    public string GetImagePathDefault(string id)
    {
        return GetImagePathPlace(id, 0);
    }

    public string GetImagePathPlace(string id, int idx)
    {
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        return CloudRes.main.rootPathGameRes + "/Image/" + infoPlace.id + "/" + id + ".png";
    }
    public void ParseGameItems()
    {
        if ((listGameItems != null) && (listGameItems.Count != 0))
        {
            return;
        }

        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);

        // GameItems
        string fileName = CloudRes.main.rootPathGameRes + "/Level/" + infoPlace.id + ".json";
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
    public void ParseGameItemsDefault()
    {
        if ((listGameItemDefault != null) && (listGameItemDefault.Count != 0))
        {
            return;
        }

        int idx = 0;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);

        // GameItems
        string fileName = CloudRes.main.rootPathGameRes + "/Level/" + infoPlace.id + ".json";
        string json = FileUtil.ReadStringAsset(fileName);//((TextAsset)Resources.Load(fileName, typeof(TextAsset))).text;

        JsonData root = JsonMapper.ToObject(json);

        JsonData items = root["items"];
        for (int i = 0; i < items.Count; i++)
        {
            JsonData item = items[i];
            ItemInfo info = new ItemInfo();
            info.id = (string)item["id"];
            info.pic = GetImagePathDefault(info.id);
            listGameItemDefault.Add(info);
        }

    }
}
