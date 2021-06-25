using UnityEngine;
using DG.Tweening;

public class SkyboxController : MonoBehaviour
{
    public Material skyboxMain;
    public Material[] skyboxs;
    public float height;

    float velocity;

    float smoothFactor = 2.5f;

    private void Start()
    {
        RenderSettings.skybox = skyboxMain;
        ChangeSkybox(skyboxs[0], 0);
        

        height = 60;
        skyboxMain.SetFloat("_height", height);
        
    }

    private void Update()
    {
        skyboxMain.SetFloat("_height", Mathf.SmoothDamp(skyboxMain.GetFloat("_height"), height, ref velocity, smoothFactor));
    }

    public void ChangeSkybox(Material material,float time)
    {
        skyboxMain.DOColor(material.GetColor("_top"), "_top", time);
        skyboxMain.DOColor(material.GetColor("_bottom"), "_bottom", time);
    }

    public void ChangeRandomSkybox()
    {
        ChangeSkybox(skyboxs[Random.Range(0, skyboxs.Length)],3.0f);
    }

    public void SetHeight(int combo)
    {
        float newHeight = 70 - (combo * 5);
        height = Mathf.Clamp(newHeight,-5.0f,60.0f);
    }
}
