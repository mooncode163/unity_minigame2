/// <summary>
/// 用于碰到线时游戏结束
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIDeadLine : MonoBehaviour
{
    public UISprite uiLine;
    public float t = 0;

        public void Awake()
    {
        this.gameObject.name = Generate.NameDeadLine;
        t = 0;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        t = 0;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        t += Time.deltaTime;
        // Debug.Log("OnTriggerStay2D t="+t);
        if (other.transform.name != Generate.NameBoardLine)
        {
            UIMergeItem ui = other.gameObject.GetComponent<UIMergeItem>();
            if(ui!=null)
            {
                if(ui.isNew)
                {
                    t =0;
                }
                if (t >= 2.0f)
                    {
                        // GameObject.Find("CodeControl").GetComponent<ScoreControl>().SaveScore();//保存分数
                        // SceneManager.LoadScene("Over");//切换场景
                        t = 0;
                        UIGameMerge.main.OnGameFinish(true);
                    }
            }

        
        }
    }

}
