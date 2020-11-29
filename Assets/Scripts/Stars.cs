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

    public void LaunchStars(int x)
    {
        foreach (Star star in stars)
        {
            star.gameObject.SetActive(false);
        }

        points = x;

        for (int i = 0; i <= points-1; i++)
            stars[i].gameObject.SetActive(true);
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
