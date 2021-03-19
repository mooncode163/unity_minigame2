/// <summary>
/// 用于碰到线时游戏结束
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
Unity中OnTriggerStay不响应问题
https://www.pianshen.com/article/32441606364/
今天在写OnTriggerStay的时候发现明明以及Stay了，但是在不动的情况下死活却不响应，后来才发现是Unity本身设置的问题，修改如下：
这里官方对于休眠的介绍是：以秒为单位的刚体在进入睡眠状态之前必须保持静止不动的时间，假设为0.5，那你不动0.5S后，物体就休眠了，所以不能触发，需要将其调高。
*/

public class UIDeadLine : MonoBehaviour
{
    public UISprite uiLine;
    public float t = 0;
    public bool isGameFail;
    public void Awake()
    {
        this.gameObject.name = GameMerge.NameDeadLine;
        t = 0;
        isGameFail = false;

    }


    void OnTriggerEnter2D(Collider2D other)
    {
        t = 0;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        t += Time.deltaTime;

        if (other.transform.name != GameMerge.NameBoardLine)
        {
            Debug.Log("OnTriggerStay2D t=" + t + " name=" + other.transform.name);
            UIMergeItem ui = other.gameObject.GetComponent<UIMergeItem>();
            if (ui != null)
            {
                if (ui.isNew)
                {
                    t = 0;
                }
                if (t >= 2.0f)
                {
                    // GameObject.Find("CodeControl").GetComponent<ScoreControl>().SaveScore();//保存分数
                    // SceneManager.LoadScene("Over");//切换场景
                    t = 0;
                    if (!isGameFail)
                    {
                        isGameFail = true;
                        UIGameMerge.main.OnGameFinish(true);
                    }
                }
            }


        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.name != GameMerge.NameBoardLine)
        {
               Debug.Log("OnTriggerStay2D exit t=" + t + " name=" + other.transform.name);
        }

    }

}
