using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mnu_TileRef : MonoBehaviour
{
    public GameObject activeTileRef;
    [SerializeField] CanvasGroup tileRefs;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] Image tileImage;
    [SerializeField] Text tileTitle;
    [SerializeField] Text tileDescription;
 
    RectTransform rect;

    [SerializeField] GameObject[] tileInfos;
    [SerializeField] AudioClip audioClip;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void OnEnable()
    {
        activateTileInfo(-1);
    }

    public void SetTileInfo()
    {
       
    }

    public void SwapTileInfo (bool showTileRefs)
    {
        StartCoroutine(FadeTileInfo(showTileRefs, Time.time, 0.5f));
    }

    // Set the active tile reference
    public void activateTileInfo (int id)
    {

/*        bool show = false;
        if (id != -1)
        {
            show = tileInfos[id].GetComponent<mnu_TileInfo>().tileDesc.activeInHierarchy;
        }

        foreach (GameObject tileInfo in tileInfos)
        {
            tileInfo.GetComponent<mnu_TileInfo>().tileDesc.SetActive(false);
        }

        if (id != -1) 
        {
            tileInfos[id].GetComponent<mnu_TileInfo>().tileDesc.SetActive(!show);
        }
*/    }

    IEnumerator FadeTileInfo(bool fadeToTileRef, float startTime, float fadeTime)
    {
        if (fadeToTileRef)
        {
            tileRefs.gameObject.SetActive(true);

            while (canvasGroup.alpha > 0f)
            {
                float deltaTime = (Time.time - startTime) / fadeTime;
                canvasGroup.alpha = Mathf.SmoothStep(1.0f, 0.0f, deltaTime);
                tileRefs.alpha = Mathf.SmoothStep(0.0f, 1.0f, deltaTime);
                yield return null;
            }

            canvasGroup.gameObject.SetActive(false);
            yield return null;
        }
        else
        {
            canvasGroup.gameObject.SetActive(true);

            while (canvasGroup.alpha > 0f)
            {
                float deltaTime = (Time.time - startTime) / fadeTime;
                canvasGroup.alpha = Mathf.SmoothStep(0.0f, 1.0f, deltaTime);
                tileRefs.alpha = Mathf.SmoothStep(1.0f, 0.0f, deltaTime);
                yield return null;
            }

            tileRefs.gameObject.SetActive(false);
            yield return null;
        }
    }
}
