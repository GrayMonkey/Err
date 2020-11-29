using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    [SerializeField] Button nextClue;
    [SerializeField] Button failQuestion;
    [SerializeField] bool scaleX;

    PlayerController playerController;
    GameOptions gameOptions;
    Player activePlayer;
    QuestionCard qCard;
    Image timerImg;
    bool timerOn;
    bool timerDelayOn;
    float delayTime = 5.0f;
    float delayTimeRem;
    float guessTime;
    float guessTimeRem;

    // Start is called before the first frame update
    void Awake()
    {
        gameOptions = GameOptions.gameOptions;
        playerController = PlayerController.playerController;
    }

    private void OnEnable()
    {
        qCard = gameObject.GetComponentInParent<QuestionCard>();
        activePlayer = playerController.activePlayer;
        guessTime = gameOptions.guessTime;
        timerImg = gameObject.GetComponent<Image>();
        timerOn = false;
    }

    // Update is called once per frame
    void Update()
    {
/*        // TODO Ad timerstartdelay to clue number
        if (timerDelayOn)
        { }
*/
        // Crease the amount of the timer panel until it is finished
        if (timerOn)
        {
            //Debug.Log(this.name +": On");
            guessTimeRem -= Time.deltaTime;
            timerImg.fillAmount= 1.0f * (guessTimeRem / guessTime);
            if (guessTimeRem < 0.0f)
                TimeOver();
        }
    }

    public void SetTimer()
    {
        guessTimeRem = guessTime;
        timerImg.fillAmount = 1.0f;
        StartDelayTimer();
        Invoke("StartTimer", delayTime);
    }

    private void StartDelayTimer()
    {
        timerDelayOn = true;
    }
    private void StartTimer()
    {
        timerOn = true;
    }

    public void ResetTimer()
    {
        if (!gameObject.activeInHierarchy)
            return;
        timerOn = false;
        timerImg.fillAmount = 0.0f;

        // In case the timer is reset before Invoke is called in SetTimer()
        CancelInvoke();
    }

    void TimeOver()
    {
        timerDelayOn = false;
        timerOn = false;
        qCard.NextClue();
    }
}
