using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetObject : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] Text label;
    [SerializeField] Text desc;

    Text subMenuInfoText;
    Player refPlayer;
    CardSet cardSet;
    Toggle toggle;

    private void Awake()
    {
        PlayerObject playerObject = this.GetComponentInParent<PlayerObject>();
        refPlayer = playerObject.refPlayer;
        toggle = this.GetComponentInParent<Toggle>();
        subMenuInfoText = playerObject.subMenuInfoText;
    }

    public void SetUp (CardSet cSet)
    {
        cardSet = cSet;
        icon.sprite = cSet.cardSetIcon;
        label.text = cSet.cardSetName;
        desc.text = cSet.cardSetDesc;
        toggle.isOn = false;
        subMenuInfoText.text = "Select CardSet(s)... (" + refPlayer.cardSets.Count.ToString() + ")";

        if(refPlayer.cardSets.Contains(cardSet))
        {
            toggle.isOn = true;
        }
    }

    public void AddCardSet ()
    {
        bool add = this.GetComponentInParent<Toggle>().isOn;
        if(add)
        {
            if(!refPlayer.cardSets.Contains(cardSet))
            {
                refPlayer.cardSets.Add(cardSet);
            }
        } else {
            if(refPlayer.cardSets.Contains(cardSet))
            {
                refPlayer.cardSets.Remove(cardSet);
            }
        }
        ShowDescription(add);
        subMenuInfoText.text = "Select CardSet(s)... (" + refPlayer.cardSets.Count.ToString() + ")";
    }

    void ShowDescription(bool show)
    {
        desc.transform.parent.gameObject.SetActive(show);
    }

}
