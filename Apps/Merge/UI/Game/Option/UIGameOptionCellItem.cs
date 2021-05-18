
using UnityEngine;
using System.Collections;
using Tacticsoft;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

//Inherit from TableViewCell instead of MonoBehavior to use the GameObject
//containing this component as a cell in a TableView
public class UIGameOptionCellItem : UICellItemBase
{ 
    public UIImage imageBg;
    public UIImage imageItem;
    public UIButton btnSelect;
    public void Start()
    {
        this.LayOut();
    }
    public override void UpdateItem(List<object> list)
    {
        if (index < list.Count)
        {

        }
        this.LayOut();
    }

    public override void LayOut()
    {
        base.LayOut();
    }

   
}

