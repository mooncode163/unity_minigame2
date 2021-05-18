
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//  道具
public class UIPopProp : UIViewPop
{

    public enum Type
    {
        Hammer,
        Magic,
        Bomb,
    }

    public UIText textTitle;
    public UIText textGuide0;
    public UIText textGuide1;
    public UIText textGuideSelect;
    public UIImage imageIcon;
    public GameObject objItemList;
    public UIImage imageItem0;
    public UIImage imageItem1;
    public UIImage imageItem2;
    public UIImage imageItem3;
    public UIImage imageItem4;

    public UIImage imageSelect;
    public List<UIImage> listItem = new List<UIImage>();


    public Type type;
    public int indexSelect;

    public string idChangeTo;
    public void Awake()
    {
        base.Awake();

        listItem.Clear();
        listItem.Add(imageItem0);
        listItem.Add(imageItem1);
        listItem.Add(imageItem2);
        listItem.Add(imageItem3);
        listItem.Add(imageItem4);

        for (int i = 0; i < listItem.Count; i++)
        {
            {
                ItemInfo info = GameLevelParse.main.GetItemInfo(i);
                string pic = GameLevelParse.main.GetImagePath(info.id);
                UIImage ui = listItem[i];
                ui.index = i;
                ui.id = info.id;
                UITouchEvent ev = ui.gameObject.AddComponent<UITouchEvent>();
                ev.callBackTouch = OnUITouchEvent;
                ui.UpdateImage(pic);
            }
        }


    }


    public void UpdateType(Type ty)
    {
        type = ty;
        objItemList.SetActive(false);
        textGuideSelect.SetActive(false);
        textGuide1.SetActive(true);
        imageSelect.SetActive(false);

        string keyImageIcon = "";
        switch (type)
        {
            case Type.Hammer:
                {
                    keyImageIcon = "Hammer";
                    textTitle.text = Language.main.GetString("Prop") + ":" + Language.main.GetString("Prop_Hammer");
                    textGuide0.text = Language.main.GetString("Prop_Hammer_Guide0");
                    textGuide1.text = Language.main.GetString("Prop_Hammer_Guide1");
                }
                break;
            case Type.Magic:
                {
                    keyImageIcon = "Magic";
                    objItemList.SetActive(true);
                    textGuideSelect.SetActive(true);
                    textGuide1.SetActive(false);
                    imageSelect.SetActive(true);

                    textTitle.text = Language.main.GetString("Prop") + ":" + Language.main.GetString("Prop_Magic");

                    textGuide0.text = Language.main.GetString("Prop_Magic_Guide0");
                    textGuide1.text = Language.main.GetString("Prop_Magic_Guide1");
                    textGuideSelect.text = textGuide1.text;

                }
                break;
            case Type.Bomb:
                {
                    keyImageIcon = "BigBomb";
                    textTitle.text = Language.main.GetString("Prop") + ":" + Language.main.GetString("Prop_BigBomb");
                    textGuide0.text = Language.main.GetString("Prop_BigBomb_Guide0");
                    textGuide1.text = Language.main.GetString("Prop_BigBomb_Guide1");
                }
                break;
        }
        imageIcon.UpdateImageByKey(keyImageIcon);

        UIGameMerge.main.game.UpdateProp(keyImageIcon);
        LayOut();

        SetSelectImage(imageItem0);
    }
    void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        switch (status)
        {

            case UITouchEvent.STATUS_Click:
                {
                    UIImage ui = ev.gameObject.GetComponent<UIImage>();
                    SetSelectImage(ui);
                }
                break;

        }

    }

    public void SetSelectImage(UIImage ui)
    {
        idChangeTo = ui.id;
        imageSelect.transform.position = ui.transform.position;
        imageSelect.transform.localScale = ui.transform.localScale * 1.15f;
    }
    public override void LayOut()
    {
        base.LayOut();
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0;
        float ratio = 0.8f;
        if (Device.isLandscape)
        {
            ratio = 0.7f;
        }

        RectTransform rctranRoot = this.GetComponent<RectTransform>();
        Vector2 sizeCanvas = AppSceneBase.main.sizeCanvas;
        {

            w = sizeCanvas.x * ratio;
            h = sizeCanvas.y * ratio;//rctran.rect.size.y * w / rctran.rect.size.x;
            rctranRoot.sizeDelta = new Vector2(w, h);

        }
        base.LayOut();

    }

    public void OnClose()
    { 
        Close();
        // UIGameMerge.main.game.ShowProp(false);

    }
    public void OnClickBtnClose()
    {
        UIGameMerge.main.gameStatus = UIGameMerge.Status.Play;
        OnClose();
    }
    public void OnClickBtnYes()
    {
        OnClose();
        UIGameMerge.main.game.ShowProp(true);
        UIGameMerge.main.OnGameProp(this, type);

    } 
     public void OnClickBtnNo()
    {
        OnClickBtnClose();
    }
}
