using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpButtonPanel : MonoBehaviour
{
    public Text keyTitle;
    public Text keyDetails;
    public Image helpImage;

    [SerializeField] Image helpIcon;

    private void Start()
    {
        helpIcon.sprite = helpImage.sprite;
        //Debug.Log("Changed images");
    }
}
