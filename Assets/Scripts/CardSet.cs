using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardSet : MonoBehaviour 
{
    [Header ("CardSet Store Info")]
    [SerializeField] string jsonFile; // JSON file for reading data
    public string cardSetProductID;   // Unique ID to identify the CardSet
    [Space (10)]

    public string cardSetTitleKey;
    public string cardSetDescKey;
    public Image cardSetIcon;
    public bool purchased = false;
    public Langs langs;
    //public Toggle selectable;
    [SerializeField] HorizontalScrollSnap hss;
    public GameObject selectedIcon;

    private List<Question> questionList;
    private Question activeQuestion;
    private int cardRange;
    private int cardsUsed = 0;

	// Use this for initialization
	void Start () 
	{
        setupData();
        CheckPurchaseFromStore();
        Random.InitState((int)System.DateTime.Now.Ticks);
        SelectCardSet(false);
	}

    private void Update()
    {
/*        // Scale the CardSet depending on how far it is form the centre
        // This doesn't work due to the anchors not being set centrally on the cardset when
        // automatically placed as a child of the Horizontal Scroll Snap content.
        RectTransform _content =transform.parent.GetComponentInParent<RectTransform>();
        if(_content.name == "Content")
        {
            TryGetComponent<RectTransform>(out RectTransform _csRect);
            float _csPos = _csRect.localPosition.x + _csRect.rect.height / 2;
            //float _step = 312.5f;
            float _contentPos = _content.localPosition.x;
            var _scale = 1.0f - (0.5f*(Mathf.Abs((_csPos + _contentPos) / hssStep)));   // Don't ask - it works!

            transform.localScale = new Vector3(_scale, _scale, 1.0f);

            Debug.Log("Content Pos: " + _content.localPosition.x.ToString() + " > " + gameObject.name + "_pos: " + _csPos.ToString() + " > Scale: " + _scale.ToString());
        }
*/    }

    // Set up the card set questions from csvFile
    void setupData()
    {
        // Add the .json file extension and combine the whole filepath name
        string jsonFileName = jsonFile + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath + "/CardSets/", jsonFileName);

        if (File.Exists(filePath))
        {
            // read the json file in to a string and fix the string for use with JsonHelper
            // then replace the file name in the string with "dataset"
            string dataAsJson = File.ReadAllText(filePath);
            dataAsJson = dataAsJson.Replace(jsonFile, "dataset");

            // pass the string through JsonHelper class and generate a list from JSON string
            questionList = JsonHelper.FromJsonList<Question>(dataAsJson);

            cardRange = questionList.Count; // maximum range is exclusive on int - see Unity random.range 
        }
    }

	public void GetQuestion()
	{
        // if all of the cards have been used reset the deck
        if (cardsUsed > cardRange)
		{
			cardsUsed = 0;
 		}

        // get a random question and copy the question to the end of the list
        // and remove it from it's orginal position
        int i = UnityEngine.Random.Range(0, cardRange - cardsUsed);
        questionList.Add(questionList[i]);
        questionList.Remove(questionList[i]);
        cardsUsed++;

        // returns cardRange because the chosen question has been moved
        // to the end of the list
        activeQuestion = questionList[cardRange - 1];
        activeQuestion.maxPoints = 4;
        GameManager.instance.activeCardSet = this;
        GameManager.instance.activeQuestion = activeQuestion;
    }

    public void CheckPurchaseFromStore()
    {
        // ToDO: get purchase data from store
        //purchased = data from store
        // Toggle _toggle = GetComponent<Toggle>();
        // _toggle.enabled = purchased;
    }

    public void SelectCardSet (bool selected)
    {
        selectedIcon.SetActive(selected);
    }
}

[System.Serializable]
public class Question
{
    public string word;
    public string clue4;
    public string clue3;
    public string clue2;
    public string clue1;
    public string credit;
    public int maxPoints;
}