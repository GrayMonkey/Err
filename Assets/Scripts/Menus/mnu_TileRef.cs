using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mnu_TileRef : MonoBehaviour
{
    public GameObject activeTileRef;

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

    // Set the active tile reference
    public void activateTileInfo (int id)
    {
        bool show = false;
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
    }
}
