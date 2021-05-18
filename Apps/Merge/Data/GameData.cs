
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    public const string Place_Custom = "Custom";
    public const float MaxSpeed = 10.0f;
    public const float MaxBounce = 1.0f;
    public const float MaxRotation = 360.0f;

    public const string ShaderCircle = "Moonma/ImageCircle";
    public float radiusCustom =0.4f;

    //初始下落速度 1-10  
    public float speed
    {
        get
        {
            float ret = 0;
            string key = "KEY_GAME_SPEED";
            ret = PlayerPrefs.GetFloat(key, 0);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_SPEED";
            PlayerPrefs.SetFloat(key, value);
        }

    }

    //反弹 1-10
    // Unity物理反弹时的反弹系数：也就是Physic Material 的Bounciness属性
    public float bounce
    {
        get
        {
            float ret = 0;
            string key = "KEY_GAME_BOUNCE";
            ret = PlayerPrefs.GetFloat(key, 0);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_BOUNCE";
            PlayerPrefs.SetFloat(key, value);
        }

    }

    //初始角度 0-360
    public float rotation
    {
        get
        {
            float ret = 0;
            string key = "KEY_GAME_ROTATION";
            ret = PlayerPrefs.GetFloat(key, 0);
            return ret;
        }

        set
        {
            string key = "KEY_GAME_ROTATION";
            PlayerPrefs.SetFloat(key, value);
        }

    }

    //  自定义目录
    public string CustomImageRootDir
    {
        get
        {
            string ret = Application.temporaryCachePath + "/CustomImage";
            FileUtil.CreateDir(ret);
            return ret;
        }
    }


  public bool HasCustomImage
    {
        get
        {
            string key = "KEY_HasCustomImage"; 
            return Common.GetBool(key,false);
        }    set
        {
            string key = "KEY_HasCustomImage"; 
            Common.SetBool(key, value);
        }
    }
    // 自定义
    public bool isCustom = false;
    public int score = 0;


    static private GameData _main = null;
    public static GameData main
    {
        get
        {
            if (_main == null)
            {
                _main = new GameData();
            }
            return _main;
        }
    }


     public bool IsCustom()
    {
        int idx = LevelManager.main.placeLevel;
        ItemInfo infoPlace = LevelManager.main.GetPlaceItemInfo(idx);
        if (infoPlace.id == GameData.Place_Custom)
        {
            return true;
        }
        return false;
    }
}
