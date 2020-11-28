using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public bool timerOn;

    [SerializeField] Button nextClue;
    [SerializeField] Button failQuestion;
    [SerializeField] Button btnTimer;
    [SerializeField] bool scaleX;

    //CardTrad cardTrad;
    PlayerController playerController;
    GameOptions gameOptions;
    Player activePlayer;
    RectTransform thisRect;
    Text countdown = null;
    float guessTimeRemaining;
    float guessTime;
    int timerCount = 0;
 
    // Start is called before the first frame update
    void Awake()
    {
        gameOptions = GameOptions.gameOptions;
        playerController = PlayerController.playerController;
    }

    private void OnEnable()
    {
        activePlayer = playerController.activePlayer;
        guessTime = gameOptions.guessTime;
//        countdown = btnTimer.GetComponentInChildren<Text>();
        timerOn = false;

        // if the player has a guess time option then use that instead
        // if (activePlayer.guessTime > -1)
        //    guessTimeRemaining = activePlayer.guessTime;
    }

    private void Start()
    {
        thisRect = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerOn)
        {
            guessTimeRemaining -= Time.deltaTime;
            float newScale = 1.0f * (guessTimeRemaining / guessTime);
            ScaleTimer(newScale);
            countdown.text = guessTimeRemaining.ToString("##");
            if (guessTimeRemaining < 0.0f)
                TimeOver();
        }
            
    }

    public void StartTimer()
    {
        timerOn = true;
        btnTimer.interactable = false;
        guessTimeRemaining = guessTime;
    }

    public void ResetTimer(float guessTime)
    {
//        btnTimer.gameObject.SetActive(false);
        if (guessTime == 0)
        {
            this.gameObject.SetActive(false);
;       }
/*        else
        {
            btnTimer.gameObject.SetActive(true);
            countdown.text = guessTime.ToString();
        }
*/
        timerOn = false;
        ScaleTimer(0.0f);
    }

    void TimeOver()
    {
        timerOn = false;
        btnTimer.interactable = true;

        // Move on the to next clue or pass to the next player if all cluse used up
        if (nextClue.IsInteractable())
        {
            nextClue.onClick.Invoke();
        }
        else if (failQuestion.IsInteractable())
        {
            failQuestion.onClick.Invoke();
        }
    }

    void ScaleTimer(float scale)
    {
        if(scaleX)
        {
            thisRect.localScale = new Vector3(scale, 1.0f, 1.0f);
        }
        else
        {
            thisRect.localScale = new Vector3(1.0f, scale, 1.0f);
        }
    }
}
