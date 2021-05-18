
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 游戏道具
public class UIToolBar : UIView
{
    public UIButton btnImageSelect;
    public void Awake()
    {
        base.Awake();
    }
    public void Start()
    {
        base.Start();

        if (!GameData.main.IsCustom())
        {
            btnImageSelect.SetActive(false);
        }
        LayOut();
    }


    public override void LayOut()
    {
        base.LayOut();

        RectTransform rctran = this.GetComponent<RectTransform>();
        float w = rctran.rect.width;
        float h = rctran.rect.height;

        UIButton btn = this.gameObject.GetComponentInChildren<UIButton>();
        RectTransform rctranBtn = btn.GetComponent<RectTransform>();

        int count = LayoutUtil.main.GetChildCount(this.gameObject, false);
        h = count * (rctranBtn.rect.height + 24);
        rctran.sizeDelta = new Vector2(w, h);

        base.LayOut();
    }
    public void ShowPop(UIPopProp.Type type)
    {
        if (!GameMerge.main.IsHasFalledBall())
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

    public void ShowImageSelect(bool isAd)
    {
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Prop;
        string strPrefab = ConfigPrefab.main.GetPrefab("UIOptionImageSelect");
        PopUpManager.main.Show<UIOptionImageSelect>(strPrefab, popup =>
                {
                    if (isAd)
                    {
                        // AdKitCommon.main.ShowAdVideo();
                    }
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
    public void OnClickBtnOptionImageSelect()
    {
        ShowImageSelect(true);
    }

    public void OnClickBtnOptiongGame()
    {
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Prop;
        string strPrefab = ConfigPrefab.main.GetPrefab("UIOptionGame");
        PopUpManager.main.Show<UIOptionGame>(strPrefab, popup =>
                {
                    // AdKitCommon.main.ShowAdVideo();
                }, popup =>
                {


                });
    }
}
