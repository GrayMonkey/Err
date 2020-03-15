using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] GameObject[] stars;
    int points = -1;
    float starDelay = 0.5f;
    float lastStar;

    //void Update()
    //{
    //    if(points>-1)
    //    {
    //        if (Time.time < lastStar + starDelay) return; 

    //        Animator animator = stars[points].GetComponentInChildren<Animator>();
    //        animator.SetTrigger("RevealStar");

    //        points--;
    //        lastStar = Time.time;
    //    }
    //}

    public void LaunchStars(int x)
    {
        foreach (GameObject star in stars) star.SetActive(false);
        points = x - 1;

        for (int i = 0; i<=points; i++)
        {
            stars[i].SetActive(true);
            Animator anim = stars[i].GetComponentInChildren<Animator>();
            anim.SetTrigger("RevealStar");
        }

        //lastStar = Time.time;
    }
}
