using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] float delayTime = 3.0f;

    GameOptions gameOptions;
    QuestionCard questionCard;
    Image timerImg;
    float delayTimeRem;
    float guessTimeRem;

    void Awake()
    {
        questionCard = QuestionCard.instance;
        gameOptions = GameOptions.instance;
        timerImg = gameObject.GetComponent<Image>();
    }

    private void OnEnable()
    {
        guessTimeRem = gameOptions.guessTime;
        delayTimeRem = delayTime;
        questionCard.btnCorrect.interactable = true;
        timerImg.fillAmount = 1.0f;
    }

    void Update()
    {
        // Start the countdown on the delay timer
        if (delayTimeRem>0)
        {
            delayTimeRem -= Time.deltaTime;
            return;
        }

        //Debug.Log(this.name +": On");
        if (guessTimeRem>0)
        {
            guessTimeRem -= Time.deltaTime;
            timerImg.fillAmount = 1.0f * (guessTimeRem / gameOptions.guessTime);
        }
        else
        {
            Debug.Log("Timer Up!");
            questionCard.btnCorrect.interactable = false;
            this.gameObject.SetActive(false);
        }
    }
}
