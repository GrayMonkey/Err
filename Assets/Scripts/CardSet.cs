using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CardSet : MonoBehaviour 
{
    [Header ("CardSet Info")]
    [SerializeField] string jsonFile;           // JSON file for reading data
    public string cardSetProductID;   // Unique ID to identify the CardSet
    [Space (10)]

    public string cardSetTitleKey;
    public string cardSetDescKey;
    public Image cardSetIcon;
    //public string cardSetCost;
    //public GameObject cardSetLangs;
    public bool purchased;
    public bool freePurchase;

    //    public bool english;
    //    public bool french;
    //    public bool german;
    //    public bool italian;
    //    public bool spanish;
    public Langs langs;

    private List<Question> questionList;
    private Question activeQuestion;
    private int cardRange;
    private int cardsUsed = 0;

	// Use this for initialization
	void Start () 
	{
        setupData();
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks);
	}

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

        // Set the CardSetIcon
        cardSetIcon = GetComponentInParent<Image>();
    }

	public void setQuestion()
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
        GameManager.gameManager.activeCardSet = this;
        GameManager.gameManager.activeQuestion = activeQuestion;
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