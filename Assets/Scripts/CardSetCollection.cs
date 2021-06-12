using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class CardSetCollection : MonoBehaviour
{
    public static CardSetCollection instance;
    public bool shopActive = false;

    GameManager gameManager;
    QuestionManager questionManager;
    LocManager locManager;
    [SerializeField] Text title;
    [SerializeField] Text cardSetsGame;
    [SerializeField] Text cardSetsShop;
    [SerializeField] Transform cardsetContent;
    [SerializeField] Text cardsetDescription;
    [SerializeField] Slider sliderGameShop;
    [SerializeField] LangFlags langFlags;
    [SerializeField] Button btnStart;
    [SerializeField] GameObject shopEmpty;

    CardSet activeCardSet;

    private void Awake()
    {
        instance = this;
    }

/*    private void OnEnable()
    {
        SetDefaultLanguage();
    }

 */   // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        questionManager = QuestionManager.instance;
        locManager = LocManager.instance;
        SetDefaultLanguage();
        ToggleShop(shopActive);
    }

    private void Update()
    {
        if (questionManager.playableCardSets.Count == 0)
            btnStart.interactable = false;
        else
            btnStart.interactable = true;
    }

    public void ToggleShop()
    {
        bool onShop = System.Convert.ToBoolean(sliderGameShop.value);
        ToggleShop(onShop);
    }
    
    public void ToggleShop (bool onShop)
    {
        shopEmpty.SetActive(false);

        if(onShop)
        {
            title.text = locManager.GetLocText("UI_CardSetPurchase");
            cardSetsShop.color = Color.black;
            cardSetsShop.fontStyle = FontStyle.Normal;
            cardSetsGame.color = Color.grey;
            cardSetsGame.fontStyle = FontStyle.Italic;
        }
        else
        {
            title.text = locManager.GetLocText("UI_CardSetSelect");
            cardSetsShop.color = Color.grey;
            cardSetsShop.fontStyle = FontStyle.Italic;
            cardSetsGame.color = Color.black;
            cardSetsGame.fontStyle = FontStyle.Normal;
        }

        shopActive = onShop;
        sliderGameShop.value = System.Convert.ToSingle(onShop); // this could cause a loop
        btnStart.gameObject.SetActive(!shopActive);
        UpdateCardSets();
    }

    public void UpdateCardSets()
    {
        int activeChildren = 0;
        shopEmpty.SetActive(false);

        foreach (CardSet cardSet in questionManager.csAll)
        {
            bool csActivate = true;
        
            // is the cardset relevant to the current game or shop filter
            csActivate = shopActive ^ cardSet.purchased;

            // if the cardset language matches the active languages then show the cardset
            if(csActivate)
                csActivate = cardSet.langs.english & langFlags.english.isOn ||
                             cardSet.langs.french & langFlags.french.isOn   ||
                             cardSet.langs.italian & langFlags.italian.isOn ||
                             cardSet.langs.spanish & langFlags.spanish.isOn;


            // set the cardset state
            if (csActivate)
            {
                cardSet.SetState();
                activeChildren++;
            }

            cardSet.gameObject.SetActive(csActivate);
        }

        // If there are no items left in the shop then show shopEmpty
        if (shopActive && activeChildren == 0)
            shopEmpty.SetActive(true);
    }

    void SetDefaultLanguage()
    {
        langFlags.english.isOn = false;
        langFlags.french.isOn  = false;
        langFlags.italian.isOn = false;
        langFlags.spanish.isOn = false;

        switch (LocManager.instance.gameLang)
        {
            case SystemLanguage.French:
                langFlags.french.isOn = true;
                break;

            case SystemLanguage.Italian:
                langFlags.italian.isOn = true;
                break;

            case SystemLanguage.Spanish:
                langFlags.spanish.isOn = true;
                break;

            default:
                langFlags.english.isOn = true;
                break;
        }
    }

    [System.Serializable]
    public class LangFlags
    {
        public Toggle english;
        public Toggle french;
        public Toggle italian;
        public Toggle spanish;
    }
}
