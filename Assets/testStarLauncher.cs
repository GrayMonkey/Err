using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testStarLauncher : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    [SerializeField] int starCount;
    [SerializeField] float starDelay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchStars()
    {
        foreach(GameObject star in stars)
        {
            star.SetActive(false);
        }

        for (int i = 0; i < starCount; i++)
        {
            stars[i].SetActive(true);
            Animator anim = stars[i].GetComponentInChildren<Animator>();
            anim.SetTrigger("RevealStar");
        }
    }
}
