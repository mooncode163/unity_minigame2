using System;
/// <summary>
/// 小球的碰撞与合成
/// </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CollisionDetection : MonoBehaviour
{

  //  private GameObject[] fruitList;//生成水果列表
    private bool isItDetected = true;//定义是否进行碰撞检测后逻辑判断
    private bool playFallingSound = false;//定义是否播放过下落声音

    // Use this for initialization
    void Start()
    {
      //  fruitList = GameObject.Find("CodeControl").GetComponent<FruitList>().fruitList;//跨脚本获取水果列表
    }

    void OnCollisionEnter2D(Collision2D other)//碰撞检测
    {
         string _tag = other.transform.name;//获取被碰撞物体的Tag

      
        //播放下落声音
        if (other.transform.name == Generate.NameDeadLine && playFallingSound == false)
        {
           // GameObject.Find("CodeControl").GetComponent<VoiceControl>().PlayTheFallingSound();//播放下落声音
            playFallingSound = true;
            AudioPlay.main.PlayFile(AppRes.AUDIO_Down);
        }
        if((other.transform.name != Generate.NameDeadLine)&&(other.transform.name != Generate.NameBoardLine))
        {
            // Debug.Log("OnCollisionEnter2D enter="+_tag);
        }
         
        if (isItDetected == true && other.transform.name == this.name && other.transform.GetComponent<CollisionDetection>().HasTheDeliveryBeenDetected() == true) //判断碰撞物体的tag是否与自身一致和是否应该检测
        {
            isItDetected = false;//不进行检测
            other.transform.GetComponent<CollisionDetection>().IgnoreDetection();//停止对方检测
            Transform v2 = other.transform;//保存被碰撞物体的位置
            //   _tag = other.transform.tag;//获取被碰撞物体的Tag
            Debug.Log("OnCollisionEnter2D other="+_tag);
            //判断是否超出最大水果限制
           // if (Convert.ToInt32(_tag) < Generate.imageKeyFruit.Length)
           string keynext = Generate.main.GetNextItem(_tag); 
               if (!Common.BlankString(keynext))
            {
                     Debug.Log("OnCollisionEnter2D keynext="+keynext);
                //在被碰撞的物体原有的位置上生成新物体
                UIMergeItem uiNext = Generate.main.CreateItem(keynext);

                float value = 3f;
                // 生成物体 使用随机防止同地点击无限堆高
                 uiNext.transform.position = v2.position + new Vector3(UnityEngine.Random.Range(-value, value) * 0.2f, UnityEngine.Random.Range(-value, value) * 0.1f, 0);//!
                
                // uiNext.transform.position = v2.position;
                uiNext.EnableGravity(true); 
                // newFruit.GetComponent<Rigidbody2D>().simulated = true;//让水果获得重力


                //播放合成声音
               // newFruit.transform.GetComponent<AudioSource>().Play();
               AudioPlay.main.PlayFile(AppRes.AUDIO_Merge);

                //增加分数
               // GameObject.Find("CodeControl").GetComponent<ScoreControl>().ScoreIncrease(10 * Convert.ToInt32(_tag));

                GameData.main.score+=10*Generate.main.GetIndexOfItem(keynext);
                UIGameMerge.main.UpdateScore();


            Destroy(this.gameObject);
            Destroy(other.gameObject);

                // GameObject.Find("CodeControl").GetComponent<SizeChange>().ShrinkingObjects(this.gameObject);//清除自身
                // GameObject.Find("CodeControl").GetComponent<SizeChange>().ShrinkingObjects(other.gameObject);//清除被碰撞物体

                if(keynext==Generate.main.GetLastItem())
                {
                    //game win 合成了大西瓜
                     UIGameMerge.main.OnGameFinish(true);
                }
            }
        }


    }

    /// <summary>
    /// 用来忽略检测
    /// </summary>
    public void IgnoreDetection()//用于忽略检测
    {
        isItDetected = false;//不进行检测
    }

    public bool HasTheDeliveryBeenDetected()
    {
        return (isItDetected);
    }

}
