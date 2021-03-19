using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHomeMerge : UIHomeBase//, ISysImageLibDelegate
{
    public UIImage imageLogo;
    public UIHomeSideBar uiHomeSideBar;

    public UIHomeCenterBar uiHomeCenterBar;
    void Awake()
    {

        base.Awake();
        // TextureUtil.UpdateRawImageTexture(imageBg, AppRes.IMAGE_HOME_BG, true);
        string appname = Common.GetAppNameDisplay();
        TextName.text = appname;

        LevelManager.main.ParseGuanka();
        ItemInfo info = GameLevelParse.main.GetLastItemInfo();
        string pic = GameLevelParse.main.GetImagePath(info.id);
        imageLogo.UpdateImage(pic);
    }

    // Use this for initialization
    void Start()
    {
        base.Start();
        LayOut();

        OnUIDidFinish(0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBase();
    }



    public override void LayOut()
    {
        base.LayOut();
        Vector2 sizeCanvas = this.frame.size;
        float x = 0, y = 0, w = 0, h = 0;
        uiHomeCenterBar.LayOut();
        base.LayOut();
    }

}
