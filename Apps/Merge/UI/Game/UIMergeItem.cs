using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;
using DG.Tweening;

public class UIMergeItem : UIView
{
    public UISprite spriteItem;
    public bool isNew = false;
    public int type;

    public void Awake()
    {
        base.Awake();
        LoadPrefab();
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();

        Vector2 bd = spriteItem.GetBoundSize();
        CircleCollider2D box = this.gameObject.GetComponent<CircleCollider2D>();
        // box.radius = bd.x / 2;

        UITouchEventWithMove ev = this.gameObject.AddComponent<UITouchEventWithMove>();
        ev.callBackTouch = OnUITouchEvent;
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
    public void EnableGravity(bool isEnable)
    {
        Rigidbody2D bd = this.gameObject.GetComponent<Rigidbody2D>();
        bd.bodyType = isEnable ? RigidbodyType2D.Dynamic : RigidbodyType2D.Static;
    }
    void OnUITouchEvent(UITouchEvent ev, PointerEventData eventData, int status)
    {
        float duration = 0.5f;
        UISprite ui = UIGameMerge.main.game.uiProp;
        Vector3 fromPos = ui.transform.position;

        Vector3 toPos = eventData.pointerCurrentRaycast.worldPosition;
        toPos.z = fromPos.z;
        switch (status)
        {
            case UITouchEvent.STATUS_TOUCH_DOWN:
                {
                    Debug.Log("OnUITouchEvent down id=" + this.id);
                }
                break;
            case UITouchEvent.STATUS_TOUCH_UP:
                {
                    Debug.Log("OnUITouchEvent up id=" + this.id);
                }
                break;
            case UITouchEvent.STATUS_Click:
                {
                    if (UIGameMerge.main.gameStatus == UIGameMerge.Status.Prop)
                    {
                        if (UIGameMerge.main.typeProp == UIPopProp.Type.Hammer)
                        {
                            ui.transform.DOMove(toPos, duration).OnComplete(() =>
                            {
                                GameMerge.main.DeleteItem(this);
                            });
                        }

                        if (UIGameMerge.main.typeProp == UIPopProp.Type.Bomb)
                        {
                            ui.transform.DOMove(toPos, duration).OnComplete(() =>
                            {
                                GameMerge.main.DeleteAllItemsOfId(this.id);
                            });

                        }
                    }
                }
                break;

        }

         if (UIGameMerge.main.gameStatus == UIGameMerge.Status.Play)
         {  
            //触摸item区域也响应小球下落
            GameMerge.main.UpdateEvent(status);
         }

    }



}
