using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class CardSet : MonoBehaviour 
{
    [Header("CardSet Store Info")]
    public string cardSetProductID;   // Unique ID to identify the CardSet
    [Space (10)]

    public string cardSetTitleKey;
    public string cardSetDescKey;
    public Image cardSetIcon;
    public bool purchased = false;
    public Langs langs;
    [SerializeField] HorizontalScrollSnap hss;
    public GameObject selectedIcon;

    private QuestionManager questionManager;
    private string jsonFile;
    private string jsonName;
    private List<Question> questionList;
    private Question activeQuestion;
    private int cardRange;
    private int cardsUsed = 0;

	// Use this for initialization
	void Start () 
	{
        questionManager = QuestionManager.instance;
        jsonName = this.gameObject.name;
        jsonFile = jsonName + ".json";
        setupData();
        CheckPurchaseFromStore();
        Random.InitState((int)System.DateTime.Now.Ticks);
        SelectCardSet(false);
	}

    // Set up the card set questions from jsonFile
    void setupData()
    {
        // Add the .json file extension and combine the whole filepath name
        //string jsonFileName = jsonFile + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath, jsonFile);

        if (File.Exists(filePath))
        {
            // read the json file in to a string and fix the string for use with JsonHelper
            // then replace the file name in the string with "dataset"
            string dataAsJson = File.ReadAllText(filePath);
            dataAsJson = dataAsJson.Replace(jsonName, "dataset");

            // pass the string through JsonHelper class and generate a list from JSON string
            questionList = JsonHelper.FromJsonList<Question>(dataAsJson);

            cardRange = questionList.Count-1; // maximum range is exclusive on int - see Unity random.range 
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
        activeQuestion = questionList[cardRange];
        activeQuestion.maxPoints = 4;
        questionManager.activeCardSet = this;
        questionManager.activeQuestion = activeQuestion;
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