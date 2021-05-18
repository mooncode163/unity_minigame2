/// <summary>
/// 使物体增大或缩小
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMerge : UIView
{
    Material matMerge;
    public ParticleSystem psMain;
    public void Awake()
    {
        base.Awake();
        matMerge = psMain.GetComponent<Renderer>().material;
        this.LayOut();
    }
    public void Start()
    {
        base.Start();
        this.LayOut();

        
    }

     public void UpdateItem(string id)
     {
          string pic = GameLevelParse.main.GetImagePath(id);
          Texture2D tex = TextureCache.main.Load(pic,false); 
          matMerge.SetTexture("_MainTex", tex);

          Invoke("DeleteParticle", 0.6f);

     }
    public void DeleteParticle()
    {

        // for (int i = 0; i < listParticle.Count; i++)
        // {
        //     GameObject obj = listParticle[i] as GameObject;
        //     DestroyImmediate(obj);
        // }

        // listParticle.Clear();

          DestroyImmediate(this.gameObject);
    }
}
