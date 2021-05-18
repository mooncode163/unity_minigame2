
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Moonma.SysImageLib;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UIOptionImageSelectCellItem : UICellItemBase
{
    public UIImage imageBg;
    public UIImage imageItem;
    public UIButton btnSelect;
    ItemInfo infoItem;
    public void Awake()
    {
        base.Awake();
    }

    public void Start()
    {
        base.Start();
        this.LayOut();
    }
    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {
            // Material mat = imageItem.image.GetComponent<Renderer>().material;
            // if (mat == null)
            // {
            //     imageItem.image.GetComponent<Renderer>().material = new Material(Shader.Find(GameData.ShaderCircle));
            // }

            infoItem = list[index] as ItemInfo;
            imageItem.isCache = false;
            imageItem.UpdateImage(GameLevelParse.main.GetCustomImagePath(infoItem.id));
            btnSelect.textTitle.text = Language.main.GetString(infoItem.id);
            this.gameObject.name = "UIOptionImageSelectCellItem_" + infoItem.id;
            UpdateCircle();
        }
        this.LayOut();
    }
    public void UpdateCircle()
    {
        Material mat = imageItem.image.material;
        if (GameLevelParse.main.IsHasCustomImage(infoItem.id))
        {
            // 显示自定义图片 
            mat.SetFloat("_Radius", GameData.main.radiusCustom);
            mat.SetInt("_enable", 1);
        }
        else
        {
            mat.SetInt("_enable", 0); 
        }
    }
    public override void LayOut()
    {
        base.LayOut();
    }

    public Texture2D CircleTexture(Texture2D tex, int w_to, int h_to, float radius)
    {
        int w = tex.width;
        int h = tex.height;
        RenderTexture rt = new RenderTexture(w_to, h_to, 0);
        //string str = FileUtil.ReadStringAsset(ShotBase.STR_DIR_ROOT_SHADER+"/ShaderRoundRect.shader");
        Material mat = new Material(Shader.Find(GameData.ShaderCircle));//
        //设置半径 最大0.5f
        // Range(0,0.5)
        mat.SetInt("_enable", 1);
        mat.SetFloat("_Radius", radius);

        Graphics.Blit(tex, rt, mat);
        Texture2D texRet = TextureUtil.RenderTexture2Texture2D(rt);
        return texRet;
    }
    void OnSysImageLibDidOpenFinish(string file)
    {

        Texture2D tex = null;
        if (Common.isAndroid)
        {
            //unity解码
            tex = LoadTexture.LoadFromFile(file);

        }
        else
        {
            tex = LoadTexture.LoadFromFile(file);
        }
        infoItem.pic = file;
        imageItem.isCache = false;
        GameData.main.isCustom = true;
        string filesave = GameLevelParse.main.GetSaveCustomImagePath(infoItem.id);
        Debug.Log("OnSysImageLibDidOpenFinish file =" + file + " infoItem.id=" + infoItem.id + " filesave=" + filesave);
        int w, h;
        w = h = 512;
        Texture2D tex1 = TextureUtil.ConvertSize(tex, w, h);
        // float r_max = 0.5f;

        // 边界显示有问题
        // Texture2D texSave = CircleTexture(tex1, w, h, GameData.main.radiusCustom);

        Texture2D texSave = tex1;

        TextureUtil.SaveTextureToFile(texSave, filesave);
        imageItem.UpdateImageTexture(texSave);
        UpdateCircle();
        GameData.main.HasCustomImage = true;
    }

    public void OnClickBtnSelect()
    {
        SysImageLib.main.SetObjectInfo(this.gameObject.name, "OnSysImageLibDidOpenFinish");
        SysImageLib.main.OpenImage();
        OnItemClick();
    }
}

