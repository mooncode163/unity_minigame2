 
using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class GameData 
{
     
  public int score;
    // {
    //     get
    //     {
    //         int ret = 0;
    //         string key = "KEY_GAME_Score";
    //         ret = PlayerPrefs.GetInt(key, 0);
    //         if (Application.isEditor)
    //         {
    //             // ret = AppRes.GOLD_INIT_VALUE;
    //         }
    //         return ret;
    //     }

    //     set
    //     {
    //         string key = "KEY_GAME_Score";
    //         PlayerPrefs.SetInt(key, value);
    //     }

    // }

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
}
