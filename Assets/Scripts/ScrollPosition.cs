using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollPosition : MonoBehaviour
{
    public float vScrollPos;
    public float hScrollPos;

    private ScrollRect scroll;

    public void SetScrollPos ()
    {
        scroll.verticalNormalizedPosition = vScrollPos;
        scroll.horizontalNormalizedPosition = hScrollPos;
    }
}
