﻿using System.Collections;
using System.Collections.Generic;
using Tacticsoft;
using UnityEngine;
using UnityEngine.UI;

public class UIImage : UIView
{
    public Image image;




    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
        base.Awake();
        string keyPic = keyImage;
        if (Device.isLandscape)
        {
            if (!Common.BlankString(keyImageH))
            {
                keyPic = keyImageH;
            }
        }

        string pic = ImageRes.main.GetImage(keyPic);

        if (!FileUtil.FileIsExist(pic))
        {

            if (Device.isLandscape)
            {
                keyPic = keyImageH2;
            }
            else
            {
                keyPic = keyImage2;
            }
        }
        UpdateImageByKey(keyPic);
    }
    // Use this for initialization
    public void Start()
    {
        base.Start();
    }

    public void UpdateImageByKey(string key)
    {
        string pic = "";
        if (!Common.isBlankString(key))
        {
            pic = ImageRes.main.GetImage(key);
        }

        if (!Common.isBlankString(pic))
        {
            UpdateImage(pic, key);
        }
    }

    // 绝对路径
    public void UpdateImage(string pic, string key = "")
    {
        string strKey = key;
        if (Common.isBlankString(key))
        {
            strKey = this.keyImage;
        }
        if (Common.isBlankString(pic))
        {
            return;
        }
        bool isBoard = ImageRes.main.IsHasBoard(strKey);
        Vector4 board = Vector4.zero;
        if (isBoard)
        {
            board = ImageRes.main.GetImageBoard(strKey);
        }
        if (board != Vector4.zero)
        {
            //  image.imagety
        }
        RectTransform rctranOrigin = this.GetComponent<RectTransform>();
        Vector2 offsetMin = rctranOrigin.offsetMin;
        Vector2 offsetMax = rctranOrigin.offsetMax;

        Texture2D tex = TextureCache.main.Load(pic);
        TextureUtil.UpdateImageTexture(image, tex, true, board);
        RectTransform rctan = this.GetComponent<RectTransform>();
        rctan.sizeDelta = new Vector2(tex.width, tex.height);
        Debug.Log("UpdateImage pic=" + pic + "isBoard=" + isBoard + " keyImage=" + strKey + " tex.width=" + tex.width);
        if ((rctan.anchorMin == new Vector2(0.5f, 0.5f)) && (rctan.anchorMax == new Vector2(0.5f, 0.5f)))
        {
        }
        else
        {
            //sizeDelta 会自动修改offsetMin和offsetMax 所以需要还原
            rctan.offsetMin = offsetMin;
            rctan.offsetMax = offsetMax;
        }
        LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }


}
