using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMod : MonoBehaviour
{
    [SerializeField] RectTransform fillRectTransform;
    [SerializeField] RectTransform textRectTransform;
    [SerializeField] Slider slider;
    bool sliderLock;

    //private Vector2 fillOffSetMin;
    //private Vector2 textOffsetMin;

    // Use this for initialization
    void Start()
    {
        //fillOffSetMin = fillRectTransform.offsetMin;
        //textOffsetMin = textRectTransform.offsetMin;
    }

    //private void OnEnable()
    //{
    //    sliderLock = GameOptions.gameOptions.sliderLock;
    //}

    // Update the fill values and reposition the hidden text
    // as the slider moves across
    public void RevealText(float sliderValue)
    {
 
        float sliderWidth = this.GetComponent<RectTransform>().rect.width;

        Vector2 rightFillAnchorX = new Vector2(sliderValue, 0.0f);
        Vector2 textAnchorX = new Vector2(-sliderWidth * sliderValue, 0.0f); // TODO This is a complete hack at the moment 
                                                                             // to get the correct values
        fillRectTransform.anchorMin = rightFillAnchorX;
        textRectTransform.offsetMin = textAnchorX;
    }

    // When the slider is release check to see if it needs to 
    // snap closed or open
    public void OnReleaseSlider()
    {
        slider.value = Mathf.Round(slider.value);   // Snap slider to On or Off

        sliderLock = GameOptions.gameOptions.sliderLock;
        if(!sliderLock)
        {
            slider.value = 0f;
        }
    }
}

