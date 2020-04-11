using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviour
{
    [SerializeField] Star[] stars;
    [SerializeField] float starDelay = 0.25f;
    float lastStar;
    int points = -1;
    int starID = 0;

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
    private void Start()
    {
        
    }


    public void LaunchStars(int x)
    {
        foreach (Star star in stars)
        {
            star.gameObject.SetActive(false);
        }

        points = x;

        for (int i = 0; i <= points-1; i++)
        {
            stars[i].gameObject.SetActive(true);
            //    Animator anim = stars[i].GetComponentInChildren<Animator>();
            //    anim.SetTrigger("RevealStar");
        }

        //lastStar = Time.time;
    }

    private void Update()
    {
        if(Time.time > lastStar+starDelay && starID<points)
        {
            stars[starID].ActivateStar();
            starID++;
            lastStar = Time.time;
        }
        else if (starID == points)
        {
            starID = 0;
            points = -1;
        }
    }
}
