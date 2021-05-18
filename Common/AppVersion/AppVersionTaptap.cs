using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using LitJson;
public class AppVersionTaptap : AppVersionBase
{
    static private AppVersionTaptap _main = null;
    public static AppVersionTaptap main
    {
        get
        {
            if (_main == null)
            {
                _main = new AppVersionTaptap();
            }
            return _main;
        }
    }
    public const string URL_HEAD_MoonCore = "http://mooncore.cn:8182/AppVersion_taptap";
    // http://mooncore.cn:8182/AppVersion_taptap?cur_version=1.2.0&package=com.moonma.hanziyuan&appid=46445
    //https://www.taptap.com/app/46445
    public override void StartParseVersion()
    {
        string strappid = Config.main.appId;

        string url = "https://www.taptap.com/app/" + strappid;
        strUrlAppstore = url;
        strUrlComment = url + "/review";
        HttpRequest http = new HttpRequest(OnHttpRequestFinished);
        
        string package=Common.GetAppPackage();

        // package= "com.moonma.mergexigua";
        // strappid = "211206";
        url = URL_HEAD_MoonCore + "?cur_version=" + Common.GetAppVersion() + "&package=" + package + "&appid=" + strappid;
        Debug.Log("Taptap url ="+url);
        http.Get(url);
    }
    public override void ParseData(byte[] data)
    {
        ParseDataFromServer(data);
    }
    public void ParseDataFromServer(byte[] data)
    {
        //utf8
        strVersionStore = Encoding.UTF8.GetString(data);
        Debug.Log("Taptap strVersionStore=" + strVersionStore);
        ParseFinished(this);
    }
    public void ParseDataFromWeb(byte[] data)
    {
        string strData = Encoding.UTF8.GetString(data);


        string ptmpupdate_start = "id=\"app-log\">";
        string ptmpupdate_end = "</div>";


        int idx = strData.IndexOf(ptmpupdate_start);
        if (idx >= 0)
        {
            string p = strData.Substring(idx + ptmpupdate_start.Length);
            idx = p.IndexOf(ptmpupdate_end);
            if (idx >= 0)
            {
                string str = p.Substring(0, idx);
                strUpdateNote = str;
            }
        }




        //<span itemprop="softwareVersion" class="info-item-content">1.0.5</span>
        string ptmpversion = "softwareVersion";
        string ptmpversion_start = "info-item-content\">";
        string ptmpversion_end = "</span>";

        idx = strData.IndexOf(ptmpversion);
        if (idx >= 0)
        {
            string ptmp = strData.Substring(idx);
            idx = ptmp.IndexOf(ptmpversion_start);
            if (idx >= 0)
            {
                string p = ptmp.Substring(idx + ptmpversion_start.Length);
                idx = p.IndexOf(ptmpversion_end);
                if (idx >= 0)
                {
                    strVersionStore = p.Substring(0, idx);
                }
            }
        }

        ParseFinished(this);

    }
}
