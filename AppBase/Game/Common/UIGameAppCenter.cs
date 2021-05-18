using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameAppCenter : UIView
{ 
    public const string AD_JSON_FILE = "app_game_page.json";

    //儿童连连乐 微信小程序id:wx3e44af039aee1b96   
    public const string APPCENTER_HTTP_URL = "https://6c69-lianlianle-shkb3-1259451541.tcb.qcloud.la/AppCenter/app_game_page.json?sign=b54abca80c058eb069fd354c8ac10402&t=1618475848";

     public UIButton btnApp; 
    HttpRequest httpReqJson;
    List<ItemInfo> listApp;
    List<HttpRequest> listHttpReqImage;
    int num_display = 3;
    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    public void Awake()
    {
          base.Awake(); 
        btnApp.SetActive(false); 

        StartParserAppList();
        LayOut();
    }
    // Use this for initialization
    public void Start()
    {
         base.Start();
        LayOut();
    }
   

    public override void LayOut()
    {
        base.LayOut();
        float x = 0, y = 0, w = 0, h = 0; 
    }

    void StartParserAppList()
    {

        bool enable = false;
        if (Common.isiOS)
        {
            enable = true;
        }
        else
        {
            if (AppVersion.appCheckHasFinished)
            {
                enable = true;
            }
        }

        if (Common.noad)
        {
            enable = false;
        }

        if (Common.isWinUWP)
        {
            enable = false;
        } 
        if (!enable)
        {
            return;
        }
        listHttpReqImage = new List<HttpRequest>();
        listApp = new List<ItemInfo>();
        string url = APPCENTER_HTTP_URL; 
        httpReqJson = new HttpRequest(OnHttpRequestFinished);
        httpReqJson.Get(url);

    }
    void parserAppList(byte[] data)
    {
        MoreAppParser.parserJson(data, listApp);
        //display
        int idx = 0;
        foreach (ItemInfo infoapp in listApp)
        {
            if (idx < num_display)
            {
                HttpRequest http = new HttpRequest(OnHttpRequestFinishedImage);
                listHttpReqImage.Add(http);
                startParserImage(http, infoapp.pic, idx);
            }

            idx++;
        }
    }

    public void startParserImage(HttpRequest http, string url, int idx)
    {
        http.index = idx;
        http.Get(url);
    }

    IEnumerator LoadIconImage(int idx, byte[] data)
    {
        yield return null;


    }
    void OnHttpRequestFinishedImage(HttpRequest req, bool isSuccess, byte[] data)
    {
        if (isSuccess)
        {
            //StartCoroutine(LoadIconImage(req.index, data));
            int idx = req.index;
            UIButton btn = btnApp; 
            if (btn != null)
            {
                // btn.GetComponent<Image>().color = Color.white;
                btn.SetActive(true);
                Texture2D tex = new Texture2D(0, 0, TextureFormat.ARGB32, false);
                tex.LoadImage(data);
                btn.imageBg.UpdateImageTexture(tex); 
            }
            LayOut();
        }


    }
    IEnumerator OnHttpRequestFinishedEnumerator(bool isSuccess, byte[] data)
    {
        yield return null;

    }

    void OnHttpRequestFinished(HttpRequest req, bool isSuccess, byte[] data)
    {
        // StartCoroutine(OnHttpRequestFinishedEnumerator(isSuccess, data));

        if (isSuccess)
        {
            parserAppList(data);

        }
        else
        {
            string filepath = Common.rootDirAppCenter + "/" + AD_JSON_FILE; 
            byte[] dataApp = FileUtil.ReadDataFromResources(filepath);
            if (dataApp != null)
            {
                parserAppList(dataApp);
            }
        }
    }

    //圆角
    Texture2D RoundRectTexture(Texture2D tex)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w, h, 0);
        string strshader = "Custom/RoundRect";
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(strshader));//
        float value = (156 * 1f / 1024);
        //Debug.Log("RoundRectTexture:value=" + value);
        //value = 0.1f;
        //设置半径 最大0.5f
        mat.SetFloat("_RADIUSBUCE", value);
        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }

    void GotoAppUrl(int idx)
    {
        if (listApp == null)
        {
            return;
        }
        ItemInfo info = listApp[idx];
        string appstorePackage = "";
        string appstore = Source.APPSTORE;
        if (Common.isAndroid)
        {
            if (Config.main.channel == Source.TAPTAP)
            {
                appstore = Source.TAPTAP;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_TAPTAP;
            }

            if (Config.main.channel == Source.XIAOMI)
            {
                appstore = Source.XIAOMI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_XIAOMI;
            }
            if (Config.main.channel == Source.HUAWEI)
            {
                appstore = Source.HUAWEI;
                appstorePackage = AppVersion.PACKAGE_APPSTORE_HUAWEI;
            }

        }
        AppVersion.main.GotoToAppstoreApp(appstore, info.id, appstorePackage, info.url);


    }


    public void OnUIParentGateDidCloseAppCenter(UIParentGate ui, bool isLongPress)
    {
        if (isLongPress)
        {
            Debug.Log("OnUIParentGateDidCloseAppCenter");
            GotoAppUrl(ParentGateViewController.main.index);
        }
    }
    public void ShowParentGate(int idx)
    {
        Debug.Log("ShowParentGate idx=" + idx);

        ParentGateViewController.main.index = idx;
        ParentGateViewController.main.Show(null, null);
        ParentGateViewController.main.ui.callbackClose = OnUIParentGateDidCloseAppCenter;

    }

    public void DoClickBtnAppIcon(int idx)
    {
        if (Config.main.APP_FOR_KIDS)
        {
            ShowParentGate(idx);
        }
        else
        {
            GotoAppUrl(idx);
        }
    }

    public void OnClickBtnApp()
    {
        DoClickBtnAppIcon(0);
    } 

}
