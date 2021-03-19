
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 游戏道具
public class UIToolBar : UIView
{

    public void Awake()
    {
        base.Awake();
    }

    public void ShowPop(UIPopProp.Type type)
    {
        if(!GameMerge.main.IsHasFalledBall())
        {
            return;
        }
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Prop;
        string strPrefab = ConfigPrefab.main.GetPrefab("UIPopProp");
        PopUpManager.main.Show<UIPopProp>(strPrefab, popup =>
                {
                popup.UpdateType(type);
                 AdKitCommon.main.ShowAdVideo();
                }, popup =>
                {


                });
    }

    // 锤子 摧毁指定球兵获得积分
    public void OnClickBtnHammer()
    {
        ShowPop(UIPopProp.Type.Hammer);
    }


    //  万能球 将下落的球变为指定类型球
    public void OnClickBtnMagic()
    {
        ShowPop(UIPopProp.Type.Magic);
    }


    // 大木zhui  摧毁所有的同类球并获得积分
    public void OnClickBtnBomb()
    {
        ShowPop(UIPopProp.Type.Bomb);
    }

}
