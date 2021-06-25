using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomize : MonoBehaviour
{
    public SkinnedMeshRenderer handL, handR;

    public void SetShirtToLeftHand(Material shirt)
    {
        SetMaterialTo(shirt, handL);
    }
    public void SetShirtToRightHand(Material shirt)
    {
        SetMaterialTo(shirt, handR);
    }
    public void SetShirtToBothHands(Material shirt)
    {
        SetMaterialTo(shirt, handL);
        SetMaterialTo(shirt, handR);
    }

    void SetMaterialTo(Material shirt, SkinnedMeshRenderer hand)
    {
        hand.sharedMaterial = shirt;
    }
}
