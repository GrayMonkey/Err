using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnOffSlider : Slider, IPointerDownHandler
{
    public override void OnPointerDown(PointerEventData eventData)
    {
        GameObject gameObject = eventData.pointerEnter;
        if (gameObject.name != "OnOffHandle")
            return;

        Slider slider = gameObject.GetComponentInParent<Slider>();
        float newValue = slider.value;
        newValue += 1.0f;
        slider.value = newValue % 2;
    }
}
