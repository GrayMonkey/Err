using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetObject : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text label;
    [SerializeField] Text desc;

    public void SetUp (CardSet cardSet)
    {
        icon.sprite = cardSet.cardSetIcon;
        label.text = cardSet.cardSetName;
        desc.text = cardSet.cardSetDesc;
    }
}
