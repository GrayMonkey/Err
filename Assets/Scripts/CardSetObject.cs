using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardSetObject : MonoBehaviour
{
    [System.Serializable]
    public struct LangFlags
    {
        public GameObject english;
        public GameObject french;
        public GameObject german;
        public GameObject italian;
        public GameObject spanish;
    }

    public CardSet cardSet;

    [SerializeField] private Image icon;
    [SerializeField] private Text label;
    [SerializeField] private Text desc;
    [SerializeField] private GameObject langs;
    [SerializeField] private LangFlags langFlags;
    Text subMenuInfoText;
    Player refPlayer;
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
        label.text = LocManager.locManager.GetLocText(cSet.cardSetNameKey);
        desc.text = LocManager.locManager.GetLocText(cSet.cardSetDescKey);
        toggle.isOn = false;
        subMenuInfoText.text = "Select CardSet(s)... (" + refPlayer.cardSets.Count.ToString() + ")";

        langFlags.english.SetActive(cSet.cardSetLangs.english);
        langFlags.french.SetActive(cSet.cardSetLangs.french);
        langFlags.german.SetActive(cSet.cardSetLangs.german);
        langFlags.italian.SetActive(cSet.cardSetLangs.italian);
        langFlags.spanish.SetActive(cSet.cardSetLangs.spanish);

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
        desc.gameObject.SetActive(show);
        langs.SetActive(show);
    }

}
