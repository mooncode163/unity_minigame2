using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using LitJson;
public class UIMergeItem : UIView
{
    public UISprite spriteItem;
    public bool isNew =false;
    public int type;
    public int index;
    public string id;
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
 
}
