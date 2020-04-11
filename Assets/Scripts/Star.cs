using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public GameObject starBody;

    [SerializeField] Animator anim;

    private void OnEnable()
    {
        starBody.SetActive(false);
    }

    public void ActivateStar()
    {
        starBody.SetActive(true);
        anim.SetTrigger("StartReveal");
    }
}
