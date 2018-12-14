#pragma warning disable 649   // Disable [SerializeField] warnings CS0649

using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CardSet : MonoBehaviour 
{
    [SerializeField] string jsonFile;			// JSON file for reading data
    [SerializeField] string cardSetName;    	// Card set name
    [SerializeField] string cardSetDesc;        // Card set description
    [SerializeField] string cardSetLang;		// Card set language
    [SerializeField] Sprite CardSetIcon;        // Image used for card set

    List<Question> questionList;
    Question activeQuestion;
    int cardRange;
    int cardsUsed = 0;

	// Use this for initialization
	void Start () 
	{
        setupData();
        Random.InitState((int)System.DateTime.Now.Ticks);
	}

    // Set up the card set questions from csvFile
    void setupData()
    {
        // Add the .json file extension and combine the whole filepath name
        jsonFile = jsonFile + ".json";
        string filePath = Path.Combine(Application.streamingAssetsPath + "/CardSets/", jsonFile);

        if (File.Exists(filePath))
        {
            // read the json file in to a string and fix the string for use with JsonHelper
            string dataAsJson = File.ReadAllText(filePath);

            // pass the string through JsonHelper class and generate a list from JSON string
            // Question[] questions = JsonHelper.FromJson<Question>(dataAsJson); 			// Removed as replaced Question array with List
            questionList = JsonHelper.FromJsonList<Question>(dataAsJson);

            cardRange = questionList.Count; // maximum range is exclusive on int - see Unity random.range 
        }
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
    public int maxPoints;
}
