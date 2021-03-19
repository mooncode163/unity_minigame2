using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotConfig
{
    public ShotDeviceInfo deviceInfo;
    public int GetTotalPage()
    {
        return 4;
    }
    public ShotItemInfo GetPage(ShotDeviceInfo dev, int idx)
    {
        ShotItemInfo info = new ShotItemInfo();
        UIViewController controller = null;
        info.isRealGameUI = true;
        if (dev.name == ScreenDeviceInfo.DEVICE_NAME_ICON)
        {
            controller = IconViewController.main;
            IconViewController.main.deviceInfo = dev;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_AD)
        {
            controller = AdHomeViewController.main;
        }
        else if (dev.name == ScreenDeviceInfo.DEVICE_NAME_COPY_RIGHT_HUAWEI)
        {
            controller = CopyRightViewController.main;
            CopyRightViewController.main.deviceInfo = dev;
        }
        else
        {

            AppSceneBase.main.OnCloseAllPopView();
            switch (idx)
            {

                case 0:
                    LevelManager.main.gameLevel = 0;

                    UIGameMerge.autoClickCount = 100;
                    controller = GameViewController.main;
                    break;
                case 1:
                    LevelManager.main.gameLevel = 0;
                    UIGameMerge.autoClickCount = 150;
                    controller = GameViewController.main;
                    break;
                case 2:
                    LevelManager.main.gameLevel = 2;
                    UIGameMerge.autoClickCount = 200;
                    //  gamewin 
                    controller = GameViewController.main;
                    break;
                case 3:
                    controller = HomeViewController.main;
                    break;
                default:
                    controller = HomeViewController.main;
                    break;


            }
        }
        info.controller = controller;

        return info;
    }

}
